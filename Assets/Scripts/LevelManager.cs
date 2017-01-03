using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public PlayerToken playerToken;
    public InputManager inputManager;
    public Transform wordHolder;

    static LevelManager _lm;
    int _keys;
    int _stars;
    Dictionary<string, WordGlue> _idToGlue = new Dictionary<string, WordGlue>();
    Queue<string> _newWords = new Queue<string>();
    List<string> _usedWords = new List<string>();

    void Awake() {
        _lm = this;
        WordGlue[] wgs = wordHolder.GetComponentsInChildren<WordGlue>();
        for (int i = 0; i < wgs.Length; i++) {
            ProcessGlue(wgs[i]);
        }
    }

    public static PlayerToken GetPlayerToken() {
        return _lm.playerToken;
    }

    public static void ToggleInput(bool b) {
        _lm.inputManager.SendInputs = b;
        _lm.playerToken.Pause(!b);
    }

    public static string GetNewID() {
        string s = _lm._newWords.Dequeue();
        _lm._usedWords.Add(s);
        return s;
    }

    public static string GetUsedID() {
        return _lm._usedWords[Random.Range(0, _lm._usedWords.Count)];
    }

    public static WordGlue GetWord(string id) {
        return _lm._idToGlue[id];
    }

    void ProcessGlue(WordGlue wg) {
        if (_idToGlue.ContainsKey(wg.name))
            return;
        _idToGlue.Add(wg.name, wg);
        _newWords.Enqueue(wg.name);
    }

    public static void AddStars(int amount) {
        if (amount < 0)
            return;
        _lm._stars += amount;
    }

    public static bool SpendStars(int amount) {
        if (_lm._stars < amount)
            return false;

        _lm._stars -= amount;
        return true;
    }

    public static void AddKey() {
        _lm._keys++;
    }

    public static bool UseKey() {
        if (_lm._keys < 1)
            return false;

        _lm._keys--;
        return true;

    }
}
