using UnityEngine;
using System.Collections;
using DigitalRuby.SoundManagerNamespace;

public class AudioManager : MonoBehaviour {

	static protected AudioManager _instance = null;
	static public AudioManager Instance { get { return AudioManager._instance; } }

	public AudioSource[] MusicAudioSources;

	public AudioSource[] InterfaceSFXSources; // 0
	public AudioSource[] MonsterSFXSources; // 1
	public AudioSource[] FanfareSFXSources; // 2
	public AudioSource[] CharacterSFXSources; // 3

	protected AudioSource[][] AllSFXSources;


	public float _musicVolume = 0.9f;
	public float _soundVolume = 1.0f;

	void Awake () {
		if (AudioManager.Instance == null)
		{
			AudioManager._instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			GameObject.Destroy(this.gameObject);
			return;
		}
			
	}

	void Start(){

		AllSFXSources = new AudioSource[4][];

		AllSFXSources [0] = InterfaceSFXSources;
		AllSFXSources [1] = MonsterSFXSources;
		AllSFXSources [2] = FanfareSFXSources;
		AllSFXSources [3] = CharacterSFXSources;

	}

	public void ToggleMusicVolume(){
		SoundManager.MusicVolume = SoundManager.MusicVolume == 0 ? _musicVolume : 0;
	}

	public void ToggleSoundVolume(){
		SoundManager.SoundVolume = SoundManager.SoundVolume == 0 ? _soundVolume : 0;
	}

	public void PlaySound(int soundTypeIndex, int soundIndex){
		AudioSource aux = AllSFXSources [soundTypeIndex] [soundIndex];
		aux.PlayOneShotSoundManaged (aux.clip);
	}

	public void PlayInterfaceSound(int soundIndex){
		AudioSource aux = AllSFXSources [0] [soundIndex];
		aux.PlayOneShotSoundManaged (aux.clip);
	}


	public void PlayMusic(int index)
	{
		MusicAudioSources[index].PlayLoopingMusicManaged(1.0f, 1.0f, true);
	}

	public void StopCurrentAudio(){
		SoundManager.StopAll ();
	}
}
