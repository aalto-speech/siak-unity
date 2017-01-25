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
    protected Quaternion _startRot;
    protected Vector3 _startPos;
    protected NoGameInteractable _noGame;
    protected Transform _parent;
    
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
        float speed = (_active) ? _floatingSpeed : _floatingSpeed;
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

    public void SetNoGameInteractable(NoGameInteractable ngi) {
        _noGame = ngi;
    }

    protected IEnumerator GoToPosition(bool toLocation, Transform target, Transform mover, Transform rotater) {
        if (toLocation) {
            _startPos = transform.position;
            _startRot = rotater.rotation;
            _parent = transform.parent;
            transform.SetParent(null, true);
        } else if (_noGame != null) {
            _startRot = model.rotation * Quaternion.AngleAxis(180.0f, rotater.up);
        }
        
        Quaternion start = (toLocation) ? _startRot : target.rotation;
        Quaternion end = (!toLocation) ? _startRot : target.rotation;

        Vector3 to = (toLocation) ? target.position : _startPos;
        Vector3 from = (!toLocation) ? target.position : _startPos;
        float tween = 0;
        while (tween < 1) {
            tween += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, tween);
            rotater.rotation = Quaternion.Lerp(start, end, tween);
            yield return new WaitForEndOfFrame();
        }

        mover.position = to;
        rotater.rotation = end;
        if (toLocation)
            yield return new WaitForSeconds(0.5f);
        else
            transform.SetParent(_parent);
        EndReached(toLocation);
    }

    protected virtual void EndReached(bool toLocation) {
    }

    public void SetStartPosition(Vector3 position) {
        _startPos = position;
    }
}