using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public GameObject home;
    public GameObject arrowForward;
    public GameObject arrowBack;
    public GameObject[] levels;

    int _index;
    
    void Awake() {
        if (GameManager.GetLevelSelect() != this) {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable() {
        UpdateScreens();
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

    public void Next() {
        _index++;
        UpdateScreens();
    }

    public void Back() {
        _index--;
        UpdateScreens();
    }

    void UpdateScreens() {
        for(int i = 0; i < levels.Length; i++) {
            levels[i].SetActive(i == _index);
        }
        if (_index == 0)
            arrowBack.SetActive(false);
        else
            arrowBack.SetActive(true);

        if (_index == levels.Length-1)
            arrowForward.SetActive(false);
        else
            arrowForward.SetActive(true);
    }
}