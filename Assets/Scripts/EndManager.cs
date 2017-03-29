using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndManager : MonoBehaviour {

    public Text beat;
    public Text stars;
    public BaseActivateable la;

	void Start () {
	    if (GameManager.TotalStars() == 200) {
            beat.gameObject.SetActive(false);
            stars.gameObject.SetActive(true);
        }
	}
	
	void Update () {
        
        for (int i = 0; i < Input.touches.Length; i++) {

            if (Input.touches[i].phase != TouchPhase.Began)
                continue;
            la.SetActivateable(true);
            la.Activate();
        }

        if (Input.GetMouseButtonDown(0)) {
            la.SetActivateable(true);
            la.Activate();
        }
    }
}
