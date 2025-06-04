using System;
using System.Collections.Generic;
using UnityEngine;

public enum Menus {
	Login,
	Register,
	MainMenu,
}

public class AppManager :  PersistentSingleton<AppManager> {

	[Header("User")]
	[SerializeField] UserInformation _user;

	[Header("Account Menu")]
	[SerializeField] BaseCanvas _loginMenu;
	[SerializeField] BaseCanvas _registerMenu;

	[Header("Main Menu")]
	[SerializeField] BaseCanvas _mainMenu;

	BaseCanvas _currentCanvas;

	void Start() {

		// if(Debug.isDebugBuild) PlayerPrefs.DeleteAll();

		TryLoadUserData();
		_currentCanvas = _mainMenu;

		if(_user.email == string.Empty) {
			_currentCanvas = _registerMenu;
		}

		_currentCanvas.Show();
	}
	
	public void ChangeCanvas(Menus canvasType) {
		if(_currentCanvas != null) {
			if(canvasType == (Menus)_currentCanvas.GetCanvasType()) return;
			_currentCanvas.Hide();
		}

		switch (canvasType) {
			case Menus.Login:
				_currentCanvas = _loginMenu;
				break;
		}

		_currentCanvas.Show();
	}

	private void TryLoadUserData() {
		try {
			UserInformation newUser = new();
			_user = newUser;

			_user.email = PlayerPrefs.GetString("Email");
			_user.name = PlayerPrefs.GetString("Username");
		}
		catch (System.Exception e) {
			Debug.Log("Fail Loading _user Data! " + e);
		}
	}

	public void SetUserEmail(string email) {
		_user.email = email;
		PlayerPrefs.SetString("Email", email);
		PlayerPrefs.Save();
	}

	public void SetUserName(string name) {
		_user.name = name;
		PlayerPrefs.SetString("Username", name);
		PlayerPrefs.Save();
	}

	public UserInformation GetUserData() { return _user; }

}

[Serializable]
public class UserInformation {
	[Header("Basic Information")]
	public string name;
	public string email;
}
