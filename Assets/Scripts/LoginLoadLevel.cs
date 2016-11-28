using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginLoadLevel : MonoBehaviour {
	
	public InputField usernameInput, passwordInput;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Login()
	{
		string username, password;
		username = usernameInput.text;
		password = usernameInput.text;
	}
}
