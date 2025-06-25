using UnityEngine;
using UnityEngine.UI;

public class SimulationOptionsMenu : BaseCanvas {
	[SerializeField] Dropdown m_characterDropdown;
	[SerializeField] Dropdown m_enviromentDropdown;

	protected override void Awake() {
		base.Awake();
		m_characterDropdown.onValueChanged.AddListener(OnCharacterChanged);
		m_enviromentDropdown.onValueChanged.AddListener(OnEnviromentChanged);
	}

	private void OnCharacterChanged(int index) {
		AppManager.Instance.SetSimulationCharacter((SimulationCharacter)index);
	}

	private void OnEnviromentChanged(int index) {
		AppManager.Instance.SetSimulationEnviroment((SimulationEnviroment)index);
	}
}