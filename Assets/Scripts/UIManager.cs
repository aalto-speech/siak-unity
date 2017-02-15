using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    static UIManager _ui;

	void Awake() {
        _ui = this;
    }

    public static UIManager GetManager() {
        return _ui;
    }
}
