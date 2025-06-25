using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : BaseCanvas {
	[Header("Inputs")]
	[SerializeField] TMP_InputField m_emailInput;
	[SerializeField] TMP_InputField m_passwordInput;
	
	[Header("Buttons")]
	[SerializeField] Button m_testButton;
	[SerializeField] Button m_loginButton;
	[SerializeField] Button m_toRegisterButton;

	protected override void Awake() {
		base.Awake();
		_type = Menus.LoginMenu;
		m_loginButton.onClick.AddListener(OnLoginButtonClicked);
		m_toRegisterButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.SigninMenu);
		});
		m_testButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.TestMenu);
		});
	}

	private async void OnLoginButtonClicked() {
		string email = m_emailInput.text;
		string password = m_passwordInput.text;

		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
			Debug.LogWarning("Username or password cannot be empty.");
			return;
		}

		UserData user = new() {
			email = email, password = password
		};
		
		// Call the login method from BackendCalls
		UserResponse res = await BackendCalls.LogIn(user, 
			OnError: error => Debug.LogError("Login failed: " + error),
			OnSuccess: response => {
				Debug.Log("Login successful: " + response.user.email);
				AppManager.Instance.SetUser(response.user);
				// AppManager.Instance.ChangeCanvas(Menus.MainMenu);
			}
		);
	}
}