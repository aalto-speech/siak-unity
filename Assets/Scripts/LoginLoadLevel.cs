using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginLoadLevel : MonoBehaviour {
	
	public InputField usernameInput, passwordInput;
    public Text changer;
    public bool inMenu;

    void Start() {
        usernameInput.text = GameManager.GetUsername();
        passwordInput.text = GameManager.GetPassword();
        if (changer != null)
            changer.text = "User: " + GameManager.GetUsername();
        if (inMenu && GameManager.GetUsername() != "")
            gameObject.SetActive(false);
    }

	public void Login()
	{
        GameManager.SetUsername(usernameInput.text);
		GameManager.SetPassword(passwordInput.text);
        gameObject.SetActive(false);
        if (changer != null)
            changer.text = "User: " + usernameInput.text;
	}
}
