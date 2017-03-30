using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    static CameraManager _cm;
    Camera _cam;
    int _shakes;

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

    public static void Shake(float intensity, float duration) {
        _cm.StartCoroutine(_cm.Shaking(intensity, duration));
    }

    IEnumerator Shaking(float intensity, float duration) {
        float a = 0;
        Transform t = transform.GetChild(0);
        if (_shakes == 0) {
            _cam.enabled = false;
            t.GetComponent<Camera>().enabled = true;
        }
        _shakes++;
        Vector3 pos = Vector3.zero;
        while (a < 1) {
            a += Time.deltaTime / duration;
            t.localPosition -= pos;
            pos = (t.up * Random.Range(-1.0f, 1.0f) + t.right * Random.Range(-1.0f, 1.0f)) * (intensity*(1-a));
            t.localPosition += pos;
            yield return new WaitForEndOfFrame();
        }
        t.localPosition = Vector3.zero;
        _shakes--;

        if (_shakes == 0) {
            _cam.enabled = true;
            t.GetComponent<Camera>().enabled = false;
        }
    }

}
