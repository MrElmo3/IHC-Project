using UnityEngine;

public class EmmiterController : MonoBehaviour {
	AudioSource _audioSource;
	Sound _sound;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}
	
	void Update(){
		if(_sound == null) return;
		if(!_audioSource.isPlaying){
			_sound = null;
			SoundPool.Instance.ReturnEmmiter(this);
		}
	}

	public void PlaySound(Sound s, float volume){
		_sound = s;
		_audioSource.clip 	= _sound.clip;
		_audioSource.volume = _sound.volume * volume;
		_audioSource.pitch 	= _sound.pitch;
		_audioSource.loop 	= _sound.loop;
		_audioSource.Play();
	}

	public void UpdateVolume(float volumen){
		_audioSource.volume = _sound.volume * volumen;
	}
}