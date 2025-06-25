using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseCanvas {
	[SerializeField] Button _startButton;
	[SerializeField] Button _optionsButton;

	protected override void Awake() {
		_type = Menus.MainMenu;
		base.Awake();
		_startButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.StartMenu);
		});
		_optionsButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.OptionsMenu);
		});
	}
}