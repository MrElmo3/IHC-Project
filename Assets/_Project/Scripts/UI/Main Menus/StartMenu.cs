using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : BaseCanvas {
	[SerializeField] Button m_startSimulationButton;
	[SerializeField] Button m_backButton;

	[SerializeField] TMP_InputField m_simulationInput;

	protected override void Awake() {
		_type = Menus.StartMenu;
		base.Awake();
		
		m_startSimulationButton.onClick.AddListener(() => {
			StartSimulation();
		});
		
		m_backButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.MainMenu);
		});
	}

	private void StartSimulation() {
		// if(string.IsNullOrEmpty(m_simulationInput.text)) {
		// 	return;
		// }
		AppManager.Instance.StartSimulation(m_simulationInput.text);		
	}
}