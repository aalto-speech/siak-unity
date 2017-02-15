using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public GameObject home;
    
    void Awake() {
        if (GameManager.GetLevelSelect() != this) {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            home.SetActive(false);
        else
            home.SetActive(true);

        if (Time.timeScale == 0)
            gameObject.SetActive(false);
        else
            Time.timeScale = 0;
    }

    void OnDisable() {
        Time.timeScale = 1;
    }

    public void Hide() {
        GameManager.GetGameManager().ToggleLevelSelect();
    }
}