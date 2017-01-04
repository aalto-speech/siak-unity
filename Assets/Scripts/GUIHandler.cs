using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {

    public Text number;
    public Transform shaker;

    float _shakeAmount = 2f;

    public void Shake(float duration) {
        StartCoroutine(ShakeElement(duration));
    }

    public void SetNumber(int value) {
        number.text = value.ToString();
    }

    public void ChangeNumberBy(int amount) {
        int initial = int.Parse(number.text);
        number.text = (initial + amount).ToString();
    }

    IEnumerator ShakeElement(float duration) {
        Vector3 startPos = shaker.position;

        while (duration > 0) {
            duration -= Time.deltaTime;
            shaker.position = startPos + new Vector3(Random.Range(-_shakeAmount, _shakeAmount), Random.Range(-_shakeAmount, _shakeAmount));
            yield return new WaitForEndOfFrame();
        }
        shaker.position = startPos;
    }
}
