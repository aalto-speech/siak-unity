using UnityEngine;
using System.Collections;

public class DelayAdvancer : MonoBehaviour {

    public float beforeStartPlayDelay;
    public float afterStartPlayDelay;
    public LevelAdvancer advancer;

    void Awake() {
        StartCoroutine(DelayRoutine());
    }

    IEnumerator DelayRoutine() {
        yield return new WaitForSeconds(beforeStartPlayDelay);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(afterStartPlayDelay);
        advancer.GoNext();
    }
}
