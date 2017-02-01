using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Mysterybox : MonoBehaviour, Interactable {

    public Waypoint hiddenPoint;
    public Mysterybox[] adjacentBoxes;
    public Transform[] pathsToRaise;
    public bool interactableAtStart;
    public Transform model;

    float _ascensionHeight = 14f;
    float _ascensionDuration = 2.2f;
    float _pathAscension = 0.3f;
    int _prevLayer;
    Vector3[] _pathPositions;

    bool _canInteract;

    void Awake() {
        _pathPositions = new Vector3[pathsToRaise.Length];
        _prevLayer = hiddenPoint.gameObject.layer;
        for (int i = 0; i < pathsToRaise.Length; i++) {
            _pathPositions[i] = pathsToRaise[i].position;
        }
    }

    public void Interact() {
        if (!_canInteract)
            return;
        LevelManager.ToggleInput(false);
        StartCoroutine(Reveal());
    }

    public void SetInteractable(bool canInteract) {
        _canInteract = canInteract;
    }

    void Start() {
        SetInteractable(interactableAtStart);
        hiddenPoint.gameObject.layer = 0;
        foreach (Transform p in pathsToRaise)
            p.position += Vector3.down * _pathAscension;
    }

    void Update() {
        if (_canInteract) {
            model.Rotate(model.up, 0.1f * Time.deltaTime * 360.0f, Space.World);
        }
    }

    IEnumerator Reveal() {
        for (int i = 0; i < _pathPositions.Length; i++) {
            if (_pathPositions[i] != pathsToRaise[i].position)
                pathsToRaise[i].position = _pathPositions[i] + Vector3.down * _pathAscension;
        }
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1) {
            float increase = Time.deltaTime / _ascensionDuration;
            t += increase;
            for (int i = 0; i < pathsToRaise.Length; i++) {
                pathsToRaise[i].position = Vector3.MoveTowards(pathsToRaise[i].position, _pathPositions[i], _pathAscension*increase);
            }
            transform.position = Vector3.Slerp(startPos, startPos + Vector3.up * _ascensionHeight, t);
            yield return new WaitForEndOfFrame();
        }

        LevelManager.GetPlayerToken().returnInteract = true;
        LevelManager.GetPlayerToken().FindPathToWaypoint(hiddenPoint);
        hiddenPoint.gameObject.layer = _prevLayer;
        foreach (Mysterybox m in adjacentBoxes)
            m.SetInteractable(true);
        Destroy(this.gameObject);
    }
}
