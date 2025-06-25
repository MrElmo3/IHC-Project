using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : BaseCanvas {

	[Header("Options Menu")]
	[SerializeField] BaseCanvas simulationOptionsMenu;
	[SerializeField] BaseCanvas soundOptionsMenu;
	
	[Header("Buttons")]
	[SerializeField] Button m_simulationButton;
	[SerializeField] Button m_soundButton;
	[SerializeField] Button m_backButton;

	protected override void Awake() {
		_type = Menus.OptionsMenu;
		base.Awake();
		
		m_simulationButton.onClick.AddListener(() => {
			simulationOptionsMenu.Show();
			soundOptionsMenu.Hide();
		});
		
		m_soundButton.onClick.AddListener(() => {
			soundOptionsMenu.Show();
			simulationOptionsMenu.Hide();
		});
		
		m_backButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.MainMenu);
		});

		OnStartHide += () => {
			simulationOptionsMenu.Show();
			soundOptionsMenu.Hide();
		};
	}
}