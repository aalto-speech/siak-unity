using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public PlayerToken playerToken;
    public InputManager inputManager;

    static LevelManager _lm;
    int _keys;
    int _stars;
    Dictionary<string, WordGlue> _idToGlue;
    Queue<string> _newWords;
    List<string> _usedWords;

    void Awake() {
        _lm = this;
    }

    public static PlayerToken GetPlayerToken() {
        return _lm.playerToken;
    }

    public static void ToggleInput(bool b) {
        _lm.inputManager.SendInputs = b;
    }
}
