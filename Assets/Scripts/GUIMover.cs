using UnityEngine;
using System.Collections;

public class GUIMover : MonoBehaviour {

    public RectTransform target;
    Vector3 _target;

    void Start() {
        _target = target.position;
    }

    public void Move(float duration) {
        if (duration == 0)
            transform.position = target.position;
        else
            StartCoroutine(MoveToNewRect(duration));
    }

    IEnumerator MoveToNewRect(float duration) {
        float a = 0;
        Vector3 startPos = transform.position;
        while (a < 1) {
            a += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, _target, a);
            yield return new WaitForEndOfFrame();
        }
    }
}
