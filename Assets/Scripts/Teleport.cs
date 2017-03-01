using UnityEngine;
using System.Collections;

public class Teleport : BaseActivateable {

    public TeleportUnlocker unlocker;
    public Waypoint target;
    public GameObject activeParticle;
    bool _canTeleport;

    void Awake() {
        if (unlocker != null)
            unlocker.SetTeleport(this);
        else
            UnlockTeleportation();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;

        if (_canTeleport)
            LevelManager.GetPlayerToken().Teleport(target);

        Deactivate();
        return true;
    }

    public void UnlockTeleportation() {
        _canTeleport = true;
        activeParticle.SetActive(true);
    }
}
