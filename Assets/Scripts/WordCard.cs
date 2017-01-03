using UnityEngine;
using System.Collections;
public class WordCard : BaseActivateable {

    public string wordID = "None";
    public MeshRenderer picture;
    public bool getUsedWord;

    float _rotationsPerSecond = 0.1f;
    Vector3 _startPos;
    Quaternion _startRot;
    PlayMakerFSM _myFSM;
    AudioSource _as;

    void Awake() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _startPos = transform.position;
        _as = GetComponent<AudioSource>();
    }

    void Start() {
    }

    void Update() {
        OnBoardRotation();
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;

        SetWord(getUsedWord);
        StartCoroutine(GoToCardPosition(true));
        LevelManager.ToggleInput(false);

        return true;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        LevelManager.ToggleInput(true);
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        return true;
    }

    void OnBoardRotation() {
        if (_active || _wayPoint == null)
            return;

        float targetY = (_canActivate) ? 120 : 0;
        Transform stand = model.GetChild(0);
        stand.localRotation = Quaternion.RotateTowards(stand.localRotation, Quaternion.Euler(0, targetY, 0), Time.deltaTime * _rotationsPerSecond * 360.0f);
        model.Rotate(Vector3.up, _rotationsPerSecond * Time.deltaTime * 360.0f, Space.World);
    }

    public void EndCardGame(int score) {
        LevelManager.AddStars(score);
        StartCoroutine(GoToCardPosition(false));
    }

    IEnumerator GoToCardPosition(bool toCardLocation) {
        Transform target = CameraManager.GetCardLocation();
        Transform stand = model.GetChild(0);

        if (toCardLocation)
            _startRot = stand.rotation;

        Quaternion start = (toCardLocation) ? _startRot : target.rotation;
        Quaternion end = (!toCardLocation) ? _startRot : target.rotation;

        Vector3 to = (toCardLocation) ? target.position : _startPos;
        Vector3 from = (!toCardLocation) ? target.position : _startPos;
        float tween = 0;
        while (tween < 1) {
            tween += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, tween);
            stand.rotation = Quaternion.Lerp(start, end, tween);
            yield return new WaitForEndOfFrame();
        }

        transform.position = to;
        stand.rotation = end;
        if (toCardLocation) {
            yield return new WaitForSeconds(0.5f);
            _as.Play();
            _myFSM.SendEvent("GameOn");
        } else
            Deactivate();
    }

    void SetWord(bool getUsed) {
        if (wordID != "None")
            return;

        wordID = (getUsed) ? LevelManager.GetUsedID() : LevelManager.GetNewID();
        WordGlue _wg = LevelManager.GetWord(wordID);
        _as.volume = _wg.volume;
        _as.clip = _wg.clip;
        picture.material.SetTexture("_MainTex", _wg.picture);
        picture.material.color = Color.white;
    }
}
