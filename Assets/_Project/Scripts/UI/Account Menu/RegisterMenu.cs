using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMenu : BaseCanvas {
	[Header("Inputs")]
	[SerializeField] TMP_InputField m_emailInput;
	[SerializeField] TMP_InputField m_passwordInput;
	[SerializeField] TMP_InputField m_confirmPasswordInput;
	[SerializeField] TMP_InputField m_firstNameInput;
	[SerializeField] TMP_InputField m_lastNameInput;
	[SerializeField] Dropdown m_difficultyPreferenceDropdown;
	[SerializeField] TMP_InputField m_anxietyLevelInput;

	[Header("Buttons")]
	[SerializeField] Button m_registerButton;

	protected override void Awake() {
		_type = Menus.SigninMenu;
		base.Awake();
		m_registerButton.onClick.AddListener(OnRegisterButtonClicked);
	}

	private void OnRegisterButtonClicked() {
		AppManager.Instance.ChangeCanvas(Menus.MainMenu);
		// string email = m_emailInput.text;
		// string password = m_passwordInput.text;
		// string confirmPassword = m_confirmPasswordInput.text;
		// string firstName = m_firstNameInput.text;
		// string lastName = m_lastNameInput.text;

		// if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || 
		// 	string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(firstName) || 
		// 	string.IsNullOrEmpty(lastName)) {
		// 	Debug.LogWarning("All fields must be filled out.");
		// 	return;
		// }

		// if (password != confirmPassword) {
		// 	Debug.LogWarning("Passwords do not match.");
		// 	return;
		// }

		// UserData user = new() {
		// 	email = email, 
		// 	password = password, 
		// 	first_name = firstName, 
		// 	last_name = lastName,
		// 	preferred_difficulty = m_difficultyPreferenceDropdown.options[m_difficultyPreferenceDropdown.value].text,
		// 	anxiety_level = int.TryParse(m_anxietyLevelInput.text, out int anxiatyLevel) ? anxiatyLevel : 0
		// };
		
		// UserResponse res = await BackendCalls.SignIn(user, 
		// 	OnError: error => Debug.LogError("Registration failed: " + error),
		// 	OnSuccess: response => {
		// 		Debug.Log("Registration successful: " + response.user.email);
		// 		AppManager.Instance.SetUser(response.user);
		// 		AppManager.Instance.ChangeCanvas(Menus.MainMenu);
		// 	}
		// );
	}
}