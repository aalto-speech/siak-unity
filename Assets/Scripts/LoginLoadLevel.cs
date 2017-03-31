using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class LoginLoadLevel : MonoBehaviour {

    public InputField usernameInput, passwordInput;
    public bool inMenu;
    public Text error;
    public Text cheat;
    
    static LoginLoadLevel _log;
    Text _changer;
    bool _waitingForServer;

    void Awake() {
        if (GameManager.GetLoginScreen() != this) { 
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if (inMenu && GameManager.GetUsername() != "") {
            Login();
            inMenu = false;
        }
    }

    void OnEnable() {
        SetUser();
    }

    public void SetUser() {
        usernameInput.text = GameManager.GetUsername();
        passwordInput.text = GameManager.GetPassword();
        GameObject go = GameObject.Find("Login Text");
        if (go != null)
            _changer = go.GetComponent<Text>();
        if (_changer != null)
            _changer.text = "User: " + GameManager.GetUsername();
    }

    public void Login() {
        if (_waitingForServer)
            return;
        else {
            if (usernameInput.text != "avaaTasot") {
                GameManager.SetUsername(usernameInput.text);
                GameManager.SetPassword(passwordInput.text);
                GameManager.GetServer().login();
                _waitingForServer = true;
                error.gameObject.SetActive(false);
                cheat.gameObject.SetActive(false);
            } else {
                int i = 0;
                int.TryParse(passwordInput.text, out i);
                GameManager.SetCheat(i);
                cheat.gameObject.SetActive(true);
                usernameInput.text = "";
                passwordInput.text = "";
            }
        }
    }

    public void GiveResponse(WWW response) {
        _waitingForServer = false;
        if (string.IsNullOrEmpty(response.error)) {
            var data = JSON.Parse(response.text);
            Debug.Log(data);
            GameManager.SetSessionData(data);
            GameManager.GetServer().SetClientId(data[0]);

            if (_changer != null)
                _changer.text = "User: " + GameManager.GetUsername();
            gameObject.SetActive(false);
        } else {
            usernameInput.text = "";
            passwordInput.text = "";
            error.gameObject.SetActive(true);
        }
    }
}