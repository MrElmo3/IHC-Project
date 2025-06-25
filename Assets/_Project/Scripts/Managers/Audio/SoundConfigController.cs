using UnityEngine;
using UnityEngine.UI;

public class SoundConfigController : MonoBehaviour {

	[SerializeField] Slider m_generalSlider;
	[SerializeField] Slider m_musicSlider;
	[SerializeField] Slider m_soundSlider;

	void Start() {
		m_generalSlider.value = AudioManager.Instance.GetGeneralVolume();
		m_musicSlider.value = AudioManager.Instance.GetMusicVolume();
		m_soundSlider.value = AudioManager.Instance.GetSoundVolume();
	}

	public void UpdateGeneralVolume(){
		AudioManager.Instance.UpdateGeneralVolume(m_generalSlider.value);
	}

	public void UpdateMusicVolume(){
		AudioManager.Instance.UpdateMusicVolume(m_musicSlider.value);
	}

	public void UpdateSoundVolume(){
		AudioManager.Instance.UpdateSoundVolume(m_soundSlider.value);
	}
}