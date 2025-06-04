using UnityEngine;
using UnityEngine.UI;

public class SoundConfigController : MonoBehaviour {

	[SerializeField] Slider _generalSlider;
	[SerializeField] Slider _musicSlider;
	[SerializeField] Slider _soundSlider;

	void Start() {
		_generalSlider.value = AudioManager.Instance.GetGeneralVolume();
		_musicSlider.value = AudioManager.Instance.GetMusicVolume();
		_soundSlider.value = AudioManager.Instance.GetSoundVolume();
	}


	public void UpdateGeneralVolume(){
		AudioManager.Instance.UpdateGeneralVolume(_generalSlider.value);
	}

	public void UpdateMusicVolume(){
		AudioManager.Instance.UpdateMusicVolume(_musicSlider.value);
	}

	public void UpdateSoundVolume(){
		AudioManager.Instance.UpdateSoundVolume(_soundSlider.value);
	}
}