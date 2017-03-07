using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceFairyController : MonoBehaviour {

    public Transform target;
    public Transform mover;
    public BaseNode addTo;
    public BaseNode toBeAdded;
    public float duration;
    public float rotSpeed;
    public float spinDistance;

    float _multiplier = 1;
    public float _rotation;
    static IceFairyController _ifc;
    public int _fairies = 0;
    List<IceFairy> _fairyList = new List<IceFairy>();

    void Awake() {
        _ifc = this;
    }

    void Update() {
        if (_fairies > 0) {
            _rotation += Time.deltaTime * rotSpeed * _multiplier;

            if (_rotation > 360.0f)
                _rotation -= 360.0f;
        }
    }

    public static void AddFairy(IceFairy ice) {
        _ifc._fairyList.Add(ice);
    }

    public static int UseFairy() {
        _ifc._fairies++;

        if (_ifc._fairies == MaxFairies()) {
            _ifc.StartCoroutine(_ifc.MoveTransform());
        }

        return _ifc._fairies;
    }

    public static int MaxFairies() {
        return _ifc._fairyList.Count;
    }

    IEnumerator MoveTransform() {
        float a = 0;
        Vector3 start = mover.position;
        LevelManager.ToggleInput(false);
        CameraManager.Shake(1.0f, duration);
        while (a < 1) {
            a += Time.deltaTime / duration;
            _multiplier = 1 + a;
            mover.position = Vector3.Lerp(start, target.position, a);
            yield return new WaitForEndOfFrame();
        }
        if (addTo != null && toBeAdded != null) {
            addTo.adjacentNodes.Add(toBeAdded);
            if (addTo.canPass)
                addTo.SpreadActive();
        }

        for (int i = 0; i < _fairyList.Count; i++)
            Destroy(_fairyList[i].gameObject);

        _fairyList.Clear();
        Destroy(this.gameObject);
        LevelManager.ToggleInput(true);
    }

    public static float SpinDistance() {
        return (2.0f - _ifc._multiplier) * _ifc.spinDistance;
    }

    public static float Rotation() {
        return _ifc._rotation;
    }
}
