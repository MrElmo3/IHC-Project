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

	[Header("Background")]
	[SerializeField] GameObject m_background;

	[Header("Account Menu")]
	[SerializeField] BaseCanvas m_loginMenu;
	[SerializeField] BaseCanvas m_registerMenu;

	[Header("Main Menu")]
	[SerializeField] BaseCanvas m_mainMenu;
	[SerializeField] BaseCanvas m_optionsMenu;
	[SerializeField] BaseCanvas m_startMenu;

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
		Instantiate(
			m_simulationCharacter == SimulationCharacter.Joe? m_joePrefab : m_louisePrefab, 
			m_characterSpawnPoint
		);

		//Start Sesion from Backend

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
			case Menus.Hided:
				HideAllCanvases();
				break;
		}
		if(m_currentCanvas == null) return;
		m_currentCanvas.Show();
		m_background.SetActive(true);
	}

	private void HideAllCanvases() {
		if(m_currentCanvas != null) {
			m_currentCanvas.Hide();
			m_background.SetActive(false);
			m_currentCanvas = null;
		}
	}

	public void StartSimulation(string context) {
		//charge the context and start the session in the backend
		SceneManager.LoadScene("Office", LoadSceneMode.Additive);
		//Instantiate Character

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
	public SimulationCharacter GetSimulationCharacter() { return m_simulationCharacter; }
	public SimulationEnviroment GetSimulationEnviroment() { return m_simulationEnviroment; }

}