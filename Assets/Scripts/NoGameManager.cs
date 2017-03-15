using UnityEngine;
using System.Collections;

public class NoGameManager : MonoBehaviour {

    public int noGameNumber;
    public GameObject button;
    public GameObject mic;
    public string wordID;
    public LevelAdvancer la;
    public bool testing;

    PlayMakerFSM _myFSM;
    int _max;
    int _count = 0;
    AudioSource _as;
    WordGlue _wg;
    bool _last;

    void Start() {
        _max = GameManager.GetNoGameCount(noGameNumber);
        if (!testing) {
            if (_max == 0) {
                _max = PlayerPrefs.GetInt("NoGame" + noGameNumber);
            } else {
                PlayerPrefs.SetInt("NoGame" + noGameNumber, _max);
            }
        } else {
            _max = 6;
        }
        _myFSM = GetComponent<PlayMakerFSM>();
        _as = GetComponent<AudioSource>();
        _myFSM.FsmVariables.FindFsmObject("button").Value = button;
        _myFSM.FsmVariables.FindFsmObject("mic").Value = mic;
        print(_max);
    }

    public void SetClip(bool second) {
        _as.clip = (!_last && second) ? _wg.foreignClip : _wg.localClip;
    }

    void SetWord(bool last = false) {
        if (!last)
            wordID = LevelManager.GetNewID();
        else
            wordID = LevelManager.GetUsedID();
        _wg = LevelManager.GetWord(wordID);
    }

    public void Next() {
        if (_count == _max + 1) {
            la.SetActivateable(true);
            la.Activate();
        } else {
            if (_count == _max) {
                _last = true;
                _as.clip = GameManager.GetFinalPreface();
            } else {
                SetWord();
                SetClip(false);
            }
            _myFSM.FsmVariables.FindFsmString("word_to_check").Value = wordID;
            _myFSM.SendEvent("GameOn");
            _count++;
        }
    }
}
