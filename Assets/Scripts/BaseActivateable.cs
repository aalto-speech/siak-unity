using UnityEngine;
using System.Collections;
using System;

public abstract class BaseActivateable : MonoBehaviour, Activateable {

    protected Waypoint _wayPoint;
    protected bool _active;
    protected bool _canActivate;

    void Start() {

    }

    public virtual bool Activate() {
        if (_active || !_canActivate)
            return false;

        _active = true;
        return true;
    }

    public virtual bool Deactivate() {
        if (!_active)
            return false;

        _active = false;
        return true;
    }

    public void SetWaypoint(Waypoint wp) {
        _wayPoint = wp;
    }
}