using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    static CameraManager _cm;
    Camera _cam;
    public Transform leftInteractionLocation;
    public Transform rightInteractionLocation;
    public Transform cardInteractionLocation;

    void Awake() {
        _cm = this;
        _cam = GetComponent<Camera>();
    }

    public static Camera GetCamera() {
        return _cm._cam;
    }

    public static Transform GetCardLocation() {
        return _cm.cardInteractionLocation;
    }

    public static Transform GetLeftLocation() {
        return _cm.leftInteractionLocation;
    }

    public static Transform GetRightLocation() {
        return _cm.rightInteractionLocation;
    }
}
