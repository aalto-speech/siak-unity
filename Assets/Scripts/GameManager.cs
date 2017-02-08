using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

//Enum's int is equal to the build index
public enum Level { None = -1, Menu = 0, Forest1 = 1, NoGame1 = 2, SandIce1 = 3, Forest5 = 4 };

public class GameManager : MonoBehaviour {

    public GameObject loader;
    public GameObject levelSelect;
    public AudioClip finalPreface;

    Dictionary<Level, int> _collectedStars = new Dictionary<Level, int>();
    Dictionary<string, int> _spentStars = new Dictionary<string, int>();
    Dictionary<Level, string> _levelID = new Dictionary<Level, string>() { { Level.Forest1, "L1" }, { Level.NoGame1, "L2" }, { Level.SandIce1, "L3" }, { Level.Forest5, "L4" } };
    static GameManager _gm;
    Level _next = Level.Forest1;
    int _completedLevels = 4;
    bool _canLevelSelect = true;
    string _username = "foo";
    string _password = "bar";

    public static GameManager GetGameManager() {
        return _gm;
    }

    void Awake() {
        if (_gm != null) {
            Destroy(this.gameObject);
            return;
        }

        _gm = this;
        DontDestroyOnLoad(this.gameObject);
        foreach (Level l in System.Enum.GetValues(typeof(Level))) {
            if (l != Level.None) {
                _collectedStars.Add(l, 0);
            }
        }
        //_username = PlayerPrefs.GetString("user", "");
        //_password = PlayerPrefs.GetString("password", "");
    }

    void Update() {
        if (_canLevelSelect && Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                Application.Quit();
            } else {
                if (!levelSelect.activeSelf)
                    levelSelect.SetActive(true);
                else {
                    levelSelect.SetActive(false);
                    ChangeLevel(Level.Menu);
                }
            }
        }
    }

    public static int TotalStars() {
        return _gm._collectedStars.Sum(x => x.Value) - _gm._spentStars.Sum(x => x.Value);
    }


    public static void ChangeLevel(Level level) {
        if (level == Level.None)
            return;
        _gm._completedLevels = Mathf.Max(_gm._completedLevels, (int)level+1);
        _gm._next = level;
        LevelManager.ToggleInput(false);
        Level current = LevelManager.GetLevel();
        if (current != Level.None) {
            int newStars = LevelManager.ThisRunStars();
            if (newStars > _gm._collectedStars[current])
                _gm._collectedStars[current] = newStars;
        }
        Instantiate(_gm.loader);
    }

    public static int GetCollectedStars(Level level) {
        return _gm._collectedStars[level];
    }

    public static bool IsSpent(string name) {
        return _gm._spentStars.ContainsKey(name);
    }

    public static void AddSpent(string name, int amount) {
        if (!IsSpent(name))
            _gm._spentStars.Add(name, amount);
    }

    public static string LevelID(Level level) {
        return _gm._levelID[level];
    }

    public static int NextLevel() {
        return (int)_gm._next;
    }

    public static int GetCompleted() {
        return _gm._completedLevels;
    }

    public static void CanLevelSelect(bool b) {
        _gm._canLevelSelect = b;
    }

    public static void SetUsername(string s) {
        _gm._username = s;
        PlayerPrefs.SetString("user", s);
    }

    public static void SetPassword(string s) {
        _gm._password = s;
        PlayerPrefs.SetString("password", s);
    }

    public static string GetUsername() {
        return _gm._username;
    }

    public static string GetPassword() {
        return _gm._password;
    }

    public static AudioClip GetFinalPreface() {
        return _gm.finalPreface;
    }
}
