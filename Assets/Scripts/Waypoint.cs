using UnityEngine;
using System.Collections;

public class Waypoint : BaseNode, Interactable {
    public BaseActivateable activateable;
    public Transform model;

    public bool _canInteract;
    bool _hasActivated;

    void Awake() {
        if (activateable != null)
            activateable.SetWaypoint(this);
        else {
            _hasActivated = true;
        }

        if (canPass == true) {
            SpreadActive();
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
        if (!_hasActivated) {
            _hasActivated = true;
            SpreadActive();
        }
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

        if (canInteract)
            model.FindChild("CanMoveRing").gameObject.SetActive(true);
    }

    public void TokenReached() {
        if (activateable != null)
            activateable.Activate();
        else
            MarkActivated();
    }
}
