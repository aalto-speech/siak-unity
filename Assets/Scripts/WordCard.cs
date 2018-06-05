using UnityEngine;
using System.Collections;
public class WordCard : BaseActivateable {

    public string wordID = "None";
    public MeshRenderer picture;
    public bool getUsedWord;
    [HideInInspector]
    public Material yellow;
    [HideInInspector]
    public Material grey;
    public MeshRenderer flag;
    
    [HideInInspector]
    public GameObject[] stars; // Potential particles

    float _rotationsPerSecond = 0.1f;
    float _standUpSpeed = 0.4f;
    PlayMakerFSM _myFSM;
    AudioSource _as;
    int _myScore;
    WordGlue _wg;
    bool _pass;

    void Awake() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _as = GetComponent<AudioSource>();
    }

    void Start() {
        SetStartPosition(transform.position);
        LevelAdvancer.AddFaceUp(this);
    }

    void Update() {
        OnBoardRotation();
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;
        /*
        _pass = true;
        int score = 5;
        if (score > _myScore) {
            LevelManager.AddStars(score - _myScore);
            _myScore = score;
        }
        Deactivate();*/
        SetWord(getUsedWord);
        StartCoroutine(GoToPosition(true, CameraManager.GetCardLocation(), transform, model.GetChild(0)));
        LevelManager.ToggleInput(false);
        GameManager.CanLevelSelect(false);
        NoGameCounter.Count();
        for (int i = 0; i < stars.Length; i++) {
            stars[i].GetComponent<MeshRenderer>().material = grey;
        }

        return true;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        LevelManager.ToggleInput(true);
        GameManager.CanLevelSelect(true);
        if (_pass) {
            LevelAdvancer.RemoveFaceUp(this);
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

    public void Shake() {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine() {
        Vector3 startPos = model.GetChild(0).localPosition;
        float a = 0;
        while (a < 1) {
            a += Time.deltaTime;
            model.GetChild(0).localPosition = startPos + Vector3.Lerp(new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f)), Vector3.zero, a);
            yield return null;
        }
        model.GetChild(0).localPosition = startPos;
    }

    public void EndCardGame(int score) {
        if (score != -5 && score != -6 ) {
            if (!getUsedWord || score > -1)
                _pass = true;
        }

        StartCoroutine(ShowStars(score));
    }
    
    IEnumerator ShowStars(int score) {
        for (int i = 0; i < score; i++) {
            Transform star = stars[i].transform;
            star.gameObject.SetActive(true);
            star.GetComponent<MeshRenderer>().material = yellow;
            float startZ = star.localPosition.z;
            float a = 0;
            while (a < 1) {
                a += Time.deltaTime / 0.3f;
                star.localPosition = Vector3.Lerp(new Vector3(star.localPosition.x,star.localPosition.y,startZ), new Vector3(star.localPosition.x, star.localPosition.y, 0),a);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (score > _myScore) {
            LevelManager.AddStars(score - _myScore);
            _myScore = score;
        }
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GoToPosition(false, CameraManager.GetCardLocation(), transform, model.GetChild(0)));
        for (int i = 0; i < stars.Length; i++) {
            stars[i].GetComponent<MeshRenderer>().material = yellow;
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
        Texture tex = (!getUsedWord && second) ? _wg.foreignFlag : _wg.localFlag;
        flag.material.SetTexture("_MainTex", tex);
    }
}
