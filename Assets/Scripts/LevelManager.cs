using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LevelManager : MonoBehaviour {

    public Level level;
    public PlayerToken playerToken;
    public InputManager inputManager;
    public GUIHandler starGUI;
    public GUIHandler greyGUI;
    public GUIHandler keyGUI;
    public client_script clientScript;

    static LevelManager _lm;
    int _keys;
    int _stars;
    int _collectedStars;
    int _thisRunStars;
    Dictionary<string, WordGlue> _idToGlue = new Dictionary<string, WordGlue>();
    Queue<string> _newWords = new Queue<string>();
    List<string> _usedWords = new List<string>();
    Transform _wordHolder;

    void Awake() {
        _lm = this;
    }

    void Start() {
        _stars += GameManager.TotalStars();
        _collectedStars = GameManager.GetCollectedStars(level);
        if (_collectedStars == 0) {
            MoveGUI(0);
        } else
            greyGUI.SetNumber(_collectedStars);
        if (starGUI != null)
            starGUI.SetNumber(_stars);
        clientScript.getWordList(GameManager.LevelID(level));
    }

    public static PlayerToken GetPlayerToken() {
        return _lm.playerToken;
    }

    public static void ToggleInput(bool b) {
        if (_lm == null || _lm.inputManager == null)
            return;
        _lm.inputManager.SendInputs = b;
        if (_lm.playerToken != null)
            _lm.playerToken.Pause(!b);
    }

    public static string GetNewID() {
        if (_lm._newWords.Count != 0) {
            string s = _lm._newWords.Dequeue();
            _lm._usedWords.Add(s);
            return s;
        } else
            return GetUsedID();
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
        _lm._thisRunStars += amount;
        int increase = Mathf.Max(0, amount - _lm._collectedStars);
        if (_lm._collectedStars != 0) {
            int change = Mathf.Min(_lm._collectedStars, amount - increase);
            _lm._collectedStars -= change;
            _lm.greyGUI.ChangeNumberBy(-change);

            if (_lm._collectedStars == 0)
                _lm.MoveGUI(1);
        }
        if (increase != 0) {
            _lm._stars += increase;
            if (_lm.starGUI != null)
                _lm.starGUI.ChangeNumberBy(amount);
        }
    }

    public static bool SpendStars(int amount) {
        /*if (_lm._stars < amount)
            return false;*/

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

    public static bool HasKey() {
        if (_lm._keys < 1)
            return false;
        return true;
    }

    public static bool UseKey() {
        if (!HasKey())
            return false;

        _lm._keys--;
        if (_lm.keyGUI != null)
            _lm.keyGUI.ChangeNumberBy(-1);
        return true;

    }

    public static void ProcessWordList(string wordList) {
        _lm.SetUpWords(wordList);
    }

    void SetUpWords(string wordList) {
        var list = JSON.Parse(wordList).AsArray;
        _wordHolder = new GameObject().transform;
        _wordHolder.name = "WordHolder";
        for (int i = 0; i < list.Count; i++) {
            string word = list[i]["word"].ToString().ToLower().TrimEnd(new char[2] { ' ', '?' });
            Object o = Resources.Load("WordGlues/" + list[i]["word"]);
            if (o != null) {
                GameObject go = Instantiate((GameObject)o);
                go.transform.SetParent(_wordHolder);
                ProcessGlue(go.GetComponent<WordGlue>());
            }
        }
    }

    public static int ThisRunStars() {
        if (_lm == null)
            return -1;
        return _lm._thisRunStars;
    }

    public static Level GetLevel() {
        if (_lm == null)
            return Level.None;
        return _lm.level;
    }

    public void MoveGUI(float duration) {
        keyGUI.GetComponent<GUIMover>().Move(duration);
        greyGUI.GetComponent<GUIMover>().Move(duration);
    }
}
