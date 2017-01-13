using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public Level level;
    public int maximumStars;
    public PlayerToken playerToken;
    public InputManager inputManager;
    public Transform wordHolder;
    public GUIHandler starGUI;
    public GUIHandler keyGUI;

    static LevelManager _lm;
    int _keys;
    int _stars = 10;
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

    void Start() {
        if (starGUI != null)
            starGUI.SetNumber(GameManager.TotalStars());
    }

    public static PlayerToken GetPlayerToken() {
        return _lm.playerToken;
    }

    public static void ToggleInput(bool b) {
        _lm.inputManager.SendInputs = b;
        if (_lm.playerToken != null)
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
        if (_lm.starGUI != null)
            _lm.starGUI.ChangeNumberBy(amount);
    }

    public static bool SpendStars(int amount) {
        if (_lm._stars < amount)
            return false;

        _lm._stars -= amount;
        if (_lm.starGUI != null)
            _lm.starGUI.ChangeNumberBy(-amount);
        
        return true;
    }

    public static void AddKey() {
        _lm._keys++;
        if (_lm.keyGUI != null)
            _lm.keyGUI.ChangeNumberBy(1);
    }

    public static bool UseKey() {
        if (_lm._keys < 1)
            return false;

        _lm._keys--;
        if (_lm.keyGUI != null)
            _lm.keyGUI.ChangeNumberBy(-1);
        return true;

    }
}
