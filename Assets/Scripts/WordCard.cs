using UnityEngine;
using System.Collections;
public class WordCard : BaseActivateable {

    public float rotationsPerSecond;
    public float floatingSpeed;
    public float floatingRadius;
    public Transform model;

    PlayMakerFSM _myFSM;
    float _piCollector;

    void Awake() {
        _myFSM = GetComponent<PlayMakerFSM>();
    }

    void Update() {
        OnBoardRotation();
        Floating();
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;
        
        return true;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        return true;
    }

    void OnBoardRotation() {
        if (_active || _wayPoint == null)
            return;

        float targetX = (_canActivate) ? 90.0f : 0f;
        model.rotation = Quaternion.RotateTowards(model.rotation, Quaternion.Euler(targetX, model.rotation.y + 1, model.rotation.z), Time.deltaTime * 360.0f * rotationsPerSecond);
    }

    void Floating() {
        if (!_canActivate) 
            return;
        _piCollector += floatingSpeed * Mathf.PI*2 * Time.deltaTime;
        model.position = Vector3.up * Mathf.Sin(_piCollector) * floatingRadius;

        if (_piCollector > Mathf.PI * 2)
            _piCollector -= Mathf.PI * 2;
    }
}
