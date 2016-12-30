using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum Level { Forest1 };

public class GameManager : MonoBehaviour {

    List<int> _collectedStars = new List<int>();
    List<int> _maximumStars = new List<int>();
    static GameManager _gm;

    public static GameManager GetGameManager() {
        return _gm;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        int max = System.Enum.GetNames(typeof(Level)).Length;
        for (int i = 0; i < max; i++) {
            _collectedStars.Add(0);
            _maximumStars.Add(0);
        }
    }
    public void SetMaximumStars(Level level, int amount) {
        _maximumStars[(int)level] = amount;
    }
}
