using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TestMenu : BaseCanvas {

	[SerializeField] Button m_TTSButton;
	[SerializeField] Button m_STTButton;

	protected override void Awake() {
		_type = Menus.TestMenu;
		base.Awake();
		
		m_TTSButton.onClick.AddListener(() => {
			VoiceController.Instance.Speak("Esto es una prueba de texto a voz. ¿Cómo estás hoy?");
		});

		m_STTButton.onClick.AddListener(async () => {
			await VoiceController.Instance.Hear();
		});
	}
}