using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class AudioManager : PersistentSingleton<AudioManager> {
	[SerializeField] GameObject soundPool;

	public Sound[] musicList;
	public Sound[] soundList;

	private float _generalVolumen = 1.0f;
	private float _musicVolume = 1.0f; //Valores de 0 a 1
	private float _soundVolume = 1.0f; //Valores de 0 a 1

	AudioSource _bgAudioSource;
	Sound _currentBG;

	Dictionary<string, Sound> _musicDict;
	Dictionary<string, Sound> _soundDict;

	protected override void Awake() {
		base.Awake();
		SetupClips();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start() {
		_bgAudioSource = GetComponent<AudioSource>();
		PlayBGM("BGGame_Base");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if(mode == LoadSceneMode.Additive) return;
		if(SoundPool.Instance != null) return;
		Instantiate(soundPool);
	}

	private void SetupClips() {
		_musicDict = musicList.ToDictionary(s => s.name, s => s);
		_soundDict = soundList.ToDictionary(s => s.name, s => s);
	}

	public void Play(string name) {
		if(!_soundDict.ContainsKey(name)){
			Debug.LogError("Sound with name " + name + " not found!");
			return;
		}
		var sound = _soundDict[name];
		var emmiter = SoundPool.Instance.PoolSoundEmmiter();
		emmiter.PlaySound(sound, _soundVolume * _generalVolumen);
	}

	public void PlayBGM(string name, float time = 0) {
		if(!_musicDict.ContainsKey(name)){
			Debug.LogError("Music with name " + name + " not found!");
			return;
		}
		var music = _musicDict[name];
		_currentBG = music;
		_bgAudioSource.clip 	= music.clip;
		_bgAudioSource.volume 	= music.volume * _musicVolume * _generalVolumen;
		_bgAudioSource.pitch 	= music.pitch;
		_bgAudioSource.loop 	= music.loop;
		_bgAudioSource.time 	= time;
		_bgAudioSource.Play();
	}

	public void UpdateBGMusic(string name, float time = 0) {
		if(_currentBG.name == name) return;
		_bgAudioSource.Stop();
		PlayBGM(name, time);
	}

	public void UpdateBGMusicInTime(string name){
		if(_currentBG.name == name) return;
		float time = _bgAudioSource.time;
		UpdateBGMusic(name, time);
	}

	public void UpdateGeneralVolume(float volume){
		if(volume < 0 || volume > 1){
			Debug.LogError("Volume " + volume + " its not in 0-1 boundaries");
			return;
		}

		_generalVolumen = volume;

		_bgAudioSource.volume = _currentBG.volume * _musicVolume * _generalVolumen;	
		SoundPool.Instance.activeObjects.ForEach( r => {
			r.GetComponent<EmmiterController>().UpdateVolume(_soundVolume * _generalVolumen);
		});
	}

	public void UpdateMusicVolume(float volume) {
		if(volume < 0 || volume > 1){
			Debug.LogError("Volume " + volume + " its not in 0-1 boundaries");
			return;
		}
		_musicVolume = volume;
		_bgAudioSource.volume = _currentBG.volume * _musicVolume * _generalVolumen;	
	}

	public void UpdateSoundVolume(float volume) {
		if(volume < 0 || volume > 1){
			Debug.LogError("Volume " + volume + " its not in 0-1 boundaries");
			return;
		}

		_soundVolume = volume;

		SoundPool.Instance.activeObjects.ForEach( r => {
			r.GetComponent<EmmiterController>().UpdateVolume(_soundVolume * _generalVolumen);
		});
	}

	private IEnumerator UpdateBGMusicWithFade(string newTheme, float fadeDuration) {
		// float t = 0.0f;
		// Sound newBGM = Array.Find(bgmSounds, bgmSounds => bgmSounds.name == newTheme);
		// if (newBGM == null) {
		// 	Debug.LogError("No se encontr� el audio!");
		// 	yield break;
		// }

		// while (t < fadeDuration) {
		// 	t += Time.deltaTime;
		// 	float normalizedTime = t / fadeDuration;
		// 	actualBGM.source.volume = Mathf.Lerp(bgMusicVolume, 0, normalizedTime);
		// 	newBGM.source.volume = Mathf.Lerp(0, bgMusicVolume, normalizedTime);
			yield return null;
		// }

		// actualBGM.source.Stop();
		// newBGM.source.volume = 0.5f;
		// PlayBGM(newTheme);
	}
	
	public void Stop() {
		// actualBGM.source.Stop();
	}
	
	public void Resume() {
		// actualBGM.source.Play();
	}
	
	public void updateWithFade(string newTheme, float fadeDuration) {
		// if (actualBGM.name != newTheme) {
		// 	StartCoroutine(UpdateBGMusicWithFade(newTheme, fadeDuration));
		// }
	}


	public float GetGeneralVolume() { return _generalVolumen; }
	public float GetMusicVolume() 	{ return _musicVolume; }
	public float GetSoundVolume() 	{ return _soundVolume; }

}
