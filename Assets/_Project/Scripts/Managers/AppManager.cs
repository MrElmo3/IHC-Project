using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Menus {
	TestMenu,
	LoginMenu,
	SigninMenu,
	MainMenu,
	OptionsMenu,
	StartMenu,
	ResultsMenu,
	SessionMenu,
	Hided
}

public enum SimulationCharacter {
	Joe,
	Louise
}

public enum SimulationEnviroment {
	Office,
	Park,
}

public class AppManager :  PersistentSingleton<AppManager> {

	[Header("User")]
	[SerializeField] UserData m_user;
	
	[Header("Sesion")]
	[SerializeField]  SessionData m_sessionData;

	[Header("Background")]
	[SerializeField] GameObject m_background;

	[Header("Account Menu")]
	[SerializeField] BaseCanvas m_loginMenu;
	[SerializeField] BaseCanvas m_registerMenu;

	[Header("Main Menu")]
	[SerializeField] BaseCanvas m_mainMenu;
	[SerializeField] BaseCanvas m_optionsMenu;
	[SerializeField] BaseCanvas m_startMenu;
	[SerializeField] BaseCanvas m_sessionMenu;

	[Header("Results Menu")]
	[SerializeField] BaseCanvas m_resultsMenu;

	[Header("Test Menu")]
	[SerializeField] BaseCanvas m_testCanvas;

	[Header("Character")]
	[SerializeField] Transform m_characterSpawnPoint;
	[SerializeField] GameObject m_joePrefab;
	[SerializeField] GameObject m_louisePrefab;

	BaseCanvas m_currentCanvas;

	#region Configuration
	private SimulationCharacter m_simulationCharacter = SimulationCharacter.Joe;
	private SimulationEnviroment m_simulationEnviroment = SimulationEnviroment.Office;
	#endregion

	void Start() {
		ChangeCanvas(Menus.LoginMenu);
	}
	
	public void ChangeCanvas(Menus canvasType) {
		if(m_currentCanvas != null) {
			if(canvasType == (Menus)m_currentCanvas.GetCanvasType()) return;
			m_currentCanvas.Hide();
		}

		switch (canvasType) {
			case Menus.LoginMenu:
				m_currentCanvas = m_loginMenu;
				break;
			case Menus.SigninMenu:
				m_currentCanvas = m_registerMenu;
				break;
			case Menus.TestMenu:
				m_currentCanvas = m_testCanvas;
				break;
			case Menus.MainMenu:
				m_currentCanvas = m_mainMenu;
				break;
			case Menus.OptionsMenu:
				m_currentCanvas = m_optionsMenu;
				break;
			case Menus.StartMenu:
				m_currentCanvas = m_startMenu;
				break;
			case Menus.ResultsMenu:
				m_currentCanvas = m_resultsMenu;
				break;
			case Menus.SessionMenu:
				m_currentCanvas = m_sessionMenu;
				break;
		}
		if(m_currentCanvas == null) return;
		m_currentCanvas.Show();
		if((Menus)m_currentCanvas.GetCanvasType() == Menus.SessionMenu  || (Menus)m_currentCanvas.GetCanvasType() == Menus.ResultsMenu) {
			m_background.SetActive(false);
		}
	}

	public async void StartSimulation(string context) {
		SessionData sessionData = new SessionData {
			user_id = m_user.id,
			scenario_id = 1, // Assuming scenario_id is 1 for the default scenario
			difficulty_level = m_user.preferred_difficulty.ToLower(),
			interviewer_avatar = m_simulationCharacter.ToString(),
			environment = m_simulationEnviroment.ToString(),
			custom_description = context
		};
		SessionResponse response = await BackendCalls.SessionCreate(sessionData);

		if (response == null) {
			Debug.LogError("Failed to create session.");
			return;
		}

		m_sessionData = response.session;
		ChangeCanvas(Menus.SessionMenu);
		SceneManager.LoadScene("Office", LoadSceneMode.Additive);

		Instantiate(
			m_simulationCharacter == SimulationCharacter.Joe? m_joePrefab : m_louisePrefab, 
			m_characterSpawnPoint
		);

		SendStartConversation();
	}

	private async void SendStartConversation() {
	if (m_sessionData == null) {
		Debug.LogError("Session data is null. Cannot start conversation.");
		return;
	}

	SessionMessage message = new SessionMessage {
		message = $@"Inicia la entrevista simulada en español, eres un ent
			revistador profesional y debes hacer preguntas al usuario para evaluar su desempeño en una entrevista de trabajo.
			estos son los datos del usuario: {JsonUtility.ToJson(m_user)}
			estos son los datos de la sesión: {JsonUtility.ToJson(m_sessionData)}
			empieza con un saludo y una pregunta introductoria.",
		speaker = "user"
	};

		await BackendCalls.SessionConversation(
			m_sessionData.id, 
			message,
			OnError: error => Debug.LogError("Error during conversation: " + error),
			OnSuccess: response => Debug.Log("Conversation started successfully: " + response.message)
		);

		QuestionResponse response = await BackendCalls.SessionQuestion(m_sessionData.id);

		Debug.Log("Response from server: " + response.question);

		VoiceController.Instance.Speak(response.question);
	}

	public void SetUser(UserData user) {
		m_user = user;
	}

	public void SetSimulationCharacter(SimulationCharacter character) {
		m_simulationCharacter = character;
	}

	public void SetSimulationEnviroment(SimulationEnviroment enviroment) {
		m_simulationEnviroment = enviroment;
	}

	public UserData GetUserData() { return m_user; }
	public SessionData GetSessionData() { return m_sessionData; }
	public SimulationCharacter GetSimulationCharacter() { return m_simulationCharacter; }
	public SimulationEnviroment GetSimulationEnviroment() { return m_simulationEnviroment; }

}