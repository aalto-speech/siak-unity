using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button levelSelect;
    public Button login;

	void Start () {
        levelSelect.onClick.AddListener(EnableLevelSelect);
        login.onClick.AddListener(EnableLogin);
	}

    void EnableLevelSelect() {
        GameManager.GetLevelSelect().gameObject.SetActive(true);
    }

    void EnableLogin() {
        GameManager.GetLoginScreen().gameObject.SetActive(true);
    }
}
