using UnityEngine;
using System.Collections;

public class NoGameManager : MonoBehaviour {

    public int noGameNumber;
    public GameObject button;
    public GameObject mic;
    public string wordID;
    public LevelAdvancer la;

    PlayMakerFSM _myFSM;
    int _max;
    int _count = 0;
    AudioSource _as;
    WordGlue _wg;

    void Start() {
        _max = GameManager.GetNoGameCount(noGameNumber);
        if (_max == 0) {
            _max = PlayerPrefs.GetInt("NoGame" + noGameNumber);
        } else {
            PlayerPrefs.SetInt("NoGame" + noGameNumber, _max);
        }
        _myFSM = GetComponent<PlayMakerFSM>();
        _as = GetComponent<AudioSource>();
        _myFSM.FsmVariables.FindFsmObject("button").Value = button;
        _myFSM.FsmVariables.FindFsmObject("mic").Value = mic;
    }
    public void SetClip(bool second) {
        _as.clip = (second) ? _wg.foreignClip : _wg.localClip;
    }

    void SetWord() {
        wordID = LevelManager.GetNewID();
        _wg = LevelManager.GetWord(wordID);
    }

    public void Next() {
        if (_count == _max) {
            la.SetActivateable(true);
            la.Activate();
        } else {
            _count++;
            SetWord();
            SetClip(false);
            _myFSM.FsmVariables.FindFsmString("word_to_check").Value = wordID;
            _myFSM.SendEvent("GameOn");
        }
    }
}
