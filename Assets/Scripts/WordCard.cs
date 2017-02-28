using UnityEngine;
using System.Collections;
public class WordCard : BaseActivateable {

    public string wordID = "None";
    public MeshRenderer picture;
    public bool getUsedWord;

    [HideInInspector]
    public GameObject[] stars; // Potential particles

    float _rotationsPerSecond = 0.1f;
    float _standUpSpeed = 0.4f;
    PlayMakerFSM _myFSM;
    AudioSource _as;
    int _myScore;
    WordGlue _wg;

    void Awake() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _as = GetComponent<AudioSource>();
    }

    void Start() {
        SetStartPosition(transform.position);
    }

    void Update() {
        OnBoardRotation();
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;

        SetWord(getUsedWord);
        StartCoroutine(GoToPosition(true, CameraManager.GetCardLocation(), transform, model.GetChild(0)));
        LevelManager.ToggleInput(false);
        GameManager.CanLevelSelect(false);

        return true;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        LevelManager.ToggleInput(true);
        GameManager.CanLevelSelect(true);
        if (_myScore > 0) {
            if (_wayPoint != null)
                _wayPoint.MarkActivated();
            if (_noGame != null)
                _noGame.MarkActivated();
        }
        return true;
    }

    void OnBoardRotation() {
        if (_active || _wayPoint == null)
            return;

        float targetY = (_canActivate) ? 120 : 0;
        Transform stand = model.GetChild(0);
        stand.localRotation = Quaternion.RotateTowards(stand.localRotation, Quaternion.Euler(0, targetY, 0), Time.deltaTime * _standUpSpeed * 360.0f);
        model.Rotate(model.forward, _rotationsPerSecond * Time.deltaTime * 360.0f, Space.World);
    }

    public void EndCardGame(int score) {
        if (score > _myScore) {
            LevelManager.AddStars(score - _myScore);
            _myScore = score;
        }
        StartCoroutine(GoToPosition(false ,CameraManager.GetCardLocation(), transform, model.GetChild(0)));

        for (int i = 0; i < stars.Length; i++) {
            bool state = (i < _myScore) ? true : false;
            stars[i].gameObject.SetActive(state);
        }
    }
    
    protected override void EndReached(bool toLocation) {
        if (toLocation) {
            if (!getUsedWord)
                SetClip(false);
            else
                _as.clip = GameManager.GetFinalPreface();
            _myFSM.FsmVariables.FindFsmString("word_to_check").Value = wordID;
            _myFSM.SendEvent("GameOn");
        } else
            Deactivate();
    }

    void SetWord(bool getUsed) {
        if (wordID != "None")
            return;

        wordID = (getUsed) ? LevelManager.GetUsedID() : LevelManager.GetNewID();
        _wg = LevelManager.GetWord(wordID);
        picture.material.SetTexture("_MainTex", _wg.picture);
        picture.material.SetTexture("_BumpMap", null);
        picture.material.color = Color.white;
    }

    public void SetClip(bool second) {
        _as.clip = (!getUsedWord && second) ? _wg.foreignClip : _wg.localClip;
    }
}
