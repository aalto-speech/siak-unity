using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable() {
        if (Time.timeScale == 0)
            gameObject.SetActive(false);
        else
            Time.timeScale = 0;
    }

    void OnDisable() {
        Time.timeScale = 1;
    }
}