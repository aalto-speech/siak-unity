using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportUnlocker : BaseActivateable {

    List<Teleport> _targets = new List<Teleport>();

    public void SetTeleport(Teleport tele) {
        _targets.Add(tele);
    }

    void Update() {
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;
        foreach(Teleport t in _targets)
            t.UnlockTeleportation();

        Destroy(this.gameObject);
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        Deactivate();
        return true;
    }
}
