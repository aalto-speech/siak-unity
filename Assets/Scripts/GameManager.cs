using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using SimpleJSON;

//Enum's int is equal to the build index
public enum Level { None = -1, Menu = 0, Forest1 = 1, Ice1 = 2, Sand1 = 3, NoBoard1 = 4, Sand5 = 5, Ice3 = 6, Forest5 = 7, SandIce1 = 8, NoBoard2 = 9, Ice4 = 10, MixAll2 = 11, Sand2 = 12, Ice5 = 13, Forest4 = 14, Ice2 = 15, Forest2 = 16, NoGame1 = 17, ForestSand1 = 18, SnowForest1 = 19, MixAll1 = 20, NoGame2 = 21, Sand4 = 22, NoBoard3 = 23, Forest3 = 24, Sand3 = 25, NoGame3 = 26, MixAll3 = 27, Begin = 98, End = 99};

public class GameManager : MonoBehaviour {

    public GameObject loader;
    public GameObject levelSelect;
    public GameObject loginScreen;
    public AudioClip finalPreface;
    [SerializeField] bool isGroupFeather;

    Dictionary<int, int> _noGameAmount = new Dictionary<int, int>() { { 1, 0 }, { 2, 0 }, { 3, 0 } };
    Dictionary<Level, int> _collectedStars = new Dictionary<Level, int>();
    Dictionary<string, int> _spentStars = new Dictionary<string, int>();
    Dictionary<Level, string> _levelWords = new Dictionary<Level, string>() { { Level.Forest1, "L1" }, { Level.Ice1, "L2" }, { Level.Sand1, "L3" }, { Level.NoBoard1, "L4" }, { Level.Sand5, "L5" }, { Level.Ice3, "L6" }, { Level.Forest5, "L7" }, { Level.SandIce1, "L8" }, { Level.NoBoard2, "L9" }, { Level.Ice4, "L10" }, { Level.MixAll2, "L11" }, { Level.Sand2, "L12" }, { Level.Ice5, "L13" }, { Level.Forest4, "L14" }, { Level.Ice2, "L15" }, { Level.Forest2, "L16" }, { Level.NoGame1, "L17" }, { Level.ForestSand1, "L18" }, { Level.SnowForest1, "L19" }, { Level.MixAll1, "L20" }, { Level.NoGame2, "L21" }, { Level.Sand4, "L22" }, { Level.NoBoard3, "L23" }, { Level.Forest3, "L24" }, { Level.Sand3, "L25" }, { Level.NoGame3, "L26" }, { Level.MixAll3, "L27" } };
    static GameManager _gm;
    Level _next = Level.Forest1;
    int _highestLevel = 1;
    bool _canLevelSelect = true;
    string _username = "";
    string _password = "";
    client_script _server;
    int[] _noGamePreceeders = new int[3] { 16, 20, 25 };
    int _cheatLevel = 0;
    bool _counting = false;
    bool hasCheckedGroup;

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
        ResetSessionData();
        SetLocalSessionData();
        //_username = PlayerPrefs.GetString("user", "");
        //_password = PlayerPrefs.GetString("password", "");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                Application.Quit();
            } else {
                ToggleLevelSelect();
            }
        }
    }


    public void ToggleLevelSelect() {
        if (!_canLevelSelect || _counting)
            return;

        UIManager ui = UIManager.GetManager();

        if (!levelSelect.activeSelf) {
            levelSelect.SetActive(true);
            if (ui != null)
                ui.gameObject.SetActive(false);
        } else {
            levelSelect.SetActive(false);
            if (ui != null)
                ui.gameObject.SetActive(true);
        }
    }

    public static int TotalStars() {
        if (_gm != null)
            return _gm._collectedStars.Sum(x => x.Value) - _gm._spentStars.Sum(x => x.Value);
        return 0;
    }


    public static void ChangeLevel(Level level, bool finished) {
        if (level == Level.None)
            return;
        if (finished) {
            _gm._highestLevel = Mathf.Max(_gm._highestLevel, (int)level);
            PlayerPrefs.SetInt("HighestCompleted", _gm._highestLevel);
        }
        _gm._next = level;
        LevelManager.ToggleInput(false);
        Level current = LevelManager.GetLevel();
        if (current != Level.None) {
            int newStars = LevelManager.ThisRunStars();
            if (newStars > _gm._collectedStars[current]) {
                _gm._collectedStars[current] = newStars;
                PlayerPrefs.SetInt("CollectedStars" + (int)current, newStars);
            }
            _gm._server.exit(newStars, LevelManager.NewSpends());
        }
        Instantiate(_gm.loader);
        _gm._counting = false;
        PlayerPrefs.Save();
    }

    public static void SetLevelStars(Level level, int stars) {
        _gm._collectedStars[level] = stars;
    }

    public static int GetCollectedStars(Level level) {
        return _gm._collectedStars[level];
    }

    public static bool IsSpent(string name) {
        return _gm._spentStars.ContainsKey(name);
    }

    public static int GetSpent(string name) {
        return _gm._spentStars[name];
    }

    public static void RemoveSpent(string name) {
        _gm._spentStars.Remove(name);
    }

    public static void AddSpent(string name, int amount) {
        if (!IsSpent(name)) {
            _gm._spentStars.Add(name, amount);
        } else if (_gm._spentStars[name] != amount) {
            _gm._spentStars[name] = amount;
        } else
            return;
        LevelManager.NewSpends().Add(name, amount.ToString());
    }

    public static string LevelWords(Level level) {
        return _gm._levelWords[level];
    }

    public static int NextLevel() {
        if (_gm._next == Level.Begin) {
            return 28;
        } else if (_gm._next == Level.End) {
            return 29;
        } else 
            return (int)_gm._next;
    }

    public static int GetCompleted() {
        return _gm._highestLevel;
    }

    public static void CanLevelSelect(bool b) {
        _gm._canLevelSelect = b;
    }

    public static void SetUsername(string s) {
        _gm._username = s;
        //PlayerPrefs.SetString("user", s);
    }

    public static void SetPassword(string s) {
        _gm._password = s;
        //PlayerPrefs.SetString("password", s);
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

    public static void SetServer(client_script cs) {
        _gm._server = cs;
    }

    public static client_script GetServer() {
        return _gm._server;
    }

    public static LoginLoadLevel GetLoginScreen() {
        return _gm.loginScreen.GetComponent<LoginLoadLevel>();
    }

    public static void SetSessionData(JSONNode data) {
        _gm.ResetSessionData();

        string[] starKeys = data["level_stars"].AsObject.GetKeys();
        foreach (string s in starKeys) {
            try {
                _gm._collectedStars[(Level)System.Enum.Parse(typeof(Level), s)] = int.Parse(data["level_stars"][s].Value);
            }
            catch (System.ArgumentException) {
            }
        }

        string[] objectKeys = data["level_objects"].AsObject.GetKeys();
        foreach (string s in objectKeys) {
            int parsed = 0;
            int.TryParse(data["level_objects"][s].Value, out parsed);
            _gm._spentStars.Add(s, parsed);
        }

        int serverNumber;
        int.TryParse(data["highest_level"].Value, out serverNumber);
        _gm._highestLevel = Mathf.Max(serverNumber, 1);
        if (!_gm.hasCheckedGroup && _gm.isGroupFeather) {
            for (int i = 0; i < _gm._noGamePreceeders.Length; i++) {
                string hold = _gm._levelWords[(Level)_gm._noGamePreceeders[i]];
                _gm._levelWords[(Level)_gm._noGamePreceeders[i]] = _gm._levelWords[(Level)(_gm._noGamePreceeders[i] + 1)];
                _gm._levelWords[(Level)(_gm._noGamePreceeders[i] + 1)] = hold;
            }
            _gm.hasCheckedGroup = true;
        }
    }

    public static void SetLocalSessionData() {
        for (int i = 1; i <= 27; ++i) {
            _gm._collectedStars[(Level)i] = PlayerPrefs.GetInt("CollectedStars" + i, 0);
            print(_gm._collectedStars[(Level)i]);
        }
        _gm._highestLevel = PlayerPrefs.GetInt("HighestCompleted", 1);
        print( _gm._highestLevel);
    }

    public static LevelSelect GetLevelSelect() {
        return _gm.levelSelect.GetComponent<LevelSelect>();
    }

    public static int GetTickets() {
        if (_gm._highestLevel > 27)
            return 6;
        if (_gm._highestLevel > 24)
            return 5;
        if (_gm._highestLevel > 18)
            return 4;
        if (_gm._highestLevel > 13)
            return 3;
        if (_gm._highestLevel > 10)
            return 2;
        if (_gm._highestLevel > 5)
            return 1;
        return 0;
    }

    void ResetSessionData() {
        _collectedStars.Clear();
        _spentStars.Clear();
        foreach (Level l in System.Enum.GetValues(typeof(Level))) {
            if (l != Level.None) {
                _collectedStars.Add(l, 0);
            }
        }
    }

    public static void AddCount(int i) {
        _gm._noGameAmount[i] = _gm._noGameAmount[i] + 1;
    }

    public static int GetNoGameCount(int i) {
        return _gm._noGameAmount[i];
    }

    public static void SetCheat(int i) {
        if (_gm != null)
            _gm._cheatLevel = i;
    }

    public static int GetCheat() {
        if (_gm != null)
            return _gm._cheatLevel;
        else
            return 0;
    }

    public static void SetCounting() {
        _gm._counting = true;
    }
}