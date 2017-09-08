using UnityEngine;
using System.Collections;

public class Waypoint : BaseNode, Interactable {
    public BaseActivateable activateable;
    public Transform model;

    public bool _canInteract;
    bool _hasActivated;

    void Awake() {
        if (activateable != null) {
            if (activateable.gameObject.activeSelf) {
                activateable.SetWaypoint(this);
            } else if (activateable.GetType() == typeof(StarPrice) && ((StarPrice)activateable).secondary != null) {
                activateable.SetWaypoint(this);
                ((StarPrice)activateable).SetUpSecondary();
            } else {
                EmptyActivateable();
            }
        } else {
            _hasActivated = true;
        }
    }

    public override void SpreadActive() {
        if (!_canInteract) {
            SetInteractable(true);
        }

        if (_hasActivated)
            base.SpreadActive();
    }

    public void MarkActivated() {
        _hasActivated = true;
        SpreadActive();
    }

    public void Interact() {
        if (!_canInteract)
            return;

        LevelManager.GetPlayerToken().FindPathToWaypoint(this);
    }

    public void SetInteractable(bool canInteract) {
        _canInteract = canInteract;
        if (activateable != null)
            activateable.SetActivateable(canInteract);

        if (canInteract && model != null)
            model.FindChild("CanMoveRing").gameObject.SetActive(true);
    }

    public void TokenReached() {
        if (activateable != null)
            activateable.Activate();
        else
            MarkActivated();
    }

    public void EmptyActivateable() {
        activateable = null;
        _hasActivated = true;
    }

    public bool CanInteract() {
        return _canInteract;
    }
}
