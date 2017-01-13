using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

//Enum's int is equal to the build index
public enum Level { None = -1, Menu = 0, Forest1 = 1, NoGame2 = 2 }; 

public class GameManager : MonoBehaviour {

    Dictionary<Level, int> _collectedStars = new Dictionary<Level, int>();
    Dictionary<Level, int> _maximumStars = new Dictionary<Level, int>();
    Dictionary<string, int> _spentStars = new Dictionary<string, int>();
    static GameManager _gm;

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
                _maximumStars.Add(l, 0);
                _collectedStars.Add(l, 0);
            }
        }
    }

    public static int TotalStars() {
        return _gm._collectedStars.Sum(x => x.Value)-_gm._spentStars.Sum(x => x.Value);
    }

    public static void SetMaximumStars(Level level, int amount) {
        _gm._maximumStars[level] = amount;
    }

    public static void ChangeLevel(Level level) {
        if (level == Level.None)
            return;
        print((int)level);
        ChangeLevel((int)level);
    }

    public static void ChangeLevel(int level) {
        SceneManager.LoadScene(level);
    }

    public static bool IsSpent(string name) {
        return _gm._spentStars.ContainsKey(name);
    }

    public static void AddSpent(string name, int amount) {
        if (!IsSpent(name))
            _gm._spentStars.Add(name, amount);
    }
}
