using UnityEngine;
using System.Collections;

public class Key : BaseActivateable {

    void Update() {
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;

        LevelManager.AddKey();
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        Destroy(this.gameObject);

        return true;
    }
}
