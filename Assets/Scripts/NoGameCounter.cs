using UnityEngine;
using System.Collections;

public class NoGameCounter : MonoBehaviour {

    static NoGameCounter _ngc;
    public int number;

    void Awake() {
        _ngc = this;
    }
    
    public static void Count() {
        if (_ngc == null)
            return;
        GameManager.AddCount(_ngc.number);
    }
}
