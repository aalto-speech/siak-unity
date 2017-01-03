using UnityEngine;
using System.Collections;
using System;

public abstract class BaseActivateable : MonoBehaviour, Activateable {

    public Transform model;

    protected float _floatingSpeed = 0.25f;
    protected float _floatingRadius = 0.25f;
    protected Waypoint _wayPoint;
    protected bool _active;
    protected bool _canActivate;
    protected float _piCollector;

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


    protected void Floating() {
        if (!_canActivate)
            return;

        float speed = (_active) ? 0.5f * _floatingSpeed : _floatingSpeed;
        float radius = (_active) ? 0.5f * _floatingRadius : _floatingRadius;

        _piCollector += speed * Mathf.PI * 2 * Time.deltaTime;
        model.localPosition = Vector3.up * Mathf.Sin(_piCollector) * radius;

        if (_piCollector > Mathf.PI * 2)
            _piCollector -= Mathf.PI * 2;
    }

    public virtual void SetActivateable(bool b) {
        _canActivate = b;
    }

    public void SetWaypoint(Waypoint wp) {
        _wayPoint = wp;
    }
}