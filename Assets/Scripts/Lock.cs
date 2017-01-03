using UnityEngine;
using System.Collections;

public class Lock : BaseActivateable {

    public BaseActivateable secondary;

    void Update() {
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;

        if (!LevelManager.UseKey())
            return false;

        if (_wayPoint != null) {
            if (secondary) {
                _wayPoint.activateable = secondary;
                secondary.SetWaypoint(_wayPoint);
            } else
                _wayPoint.MarkActivated();
        }
        Destroy(this.gameObject);

        return true;
    }
}
