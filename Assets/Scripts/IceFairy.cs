using UnityEngine;
using System.Collections;

public class IceFairy : BaseActivateable {

    int _number;
    bool _used;

    void Start() {
        IceFairyController.AddFairy(this);
    }

    public override bool Activate() {
        if (!base.Activate() || _used) {
            return false;
        }

        _number = IceFairyController.UseFairy();
        _used = true;
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        Deactivate();
        return true;
    }

    void LateUpdate() {
        if (_used) {
            Transform token = LevelManager.GetPlayerToken().model;
            transform.position = token.position + Quaternion.AngleAxis(360.0f / IceFairyController.MaxFairies() * _number + IceFairyController.Rotation(), token.up) * Vector3.forward * IceFairyController.SpinDistance();
        }
        Floating();
    }
}
