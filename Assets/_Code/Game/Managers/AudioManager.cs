﻿using UnityEngine;
using System.Collections;
using BounceDudes;
using DigitalRuby.SoundManagerNamespace;

public class AudioManager : MonoBehaviour
{

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

    void Awake()
    {
		AudioManager._instance = this;

        AllSFXSources = new AudioSource[4][];
        AllSFXSources[0] = InterfaceSFXSources;
        AllSFXSources[1] = MonsterSFXSources;
        AllSFXSources[2] = FanfareSFXSources;
        AllSFXSources[3] = CharacterSFXSources;
    }

    void Start()
    {
        if (!GameManager.Instance.SoundVolume)
            SoundManager.SoundVolume = 0f;

        if (!GameManager.Instance.MusicVolume)
            SoundManager.MusicVolume = 0f;
    }

    public void ToggleMusicVolume()
    {
        if (GameManager.Instance.MusicVolume)
            SoundManager.MusicVolume = _musicVolume;
        else
            SoundManager.MusicVolume = 0f;

    }

    public void ToggleSoundVolume()
    {
        if (GameManager.Instance.SoundVolume)
            SoundManager.SoundVolume = _soundVolume;
        else
            SoundManager.SoundVolume = 0f;

    }

	protected void PlaySound(int soundTypeIndex, int soundIndex)
    {
        this.StartCoroutine(this.PlayAfterFrame(soundTypeIndex, soundIndex));
    }

	// USE TO PLAY INTERFACES SFX
    public void PlayInterfaceSound(int soundIndex)
    {
        this.StartCoroutine(this.PlayAfterFrame(0, soundIndex));
    }

	// USE TO PLAY MONSTER SFX
	public void PlayMonsterSound(int soundIndex)
	{
		this.StartCoroutine(this.PlayAfterFrame(1, soundIndex));
	}

	// USE TO PLAY FANFARE SFX
	public void PlayFanfareSound(int soundIndex)
	{
		this.StartCoroutine(this.PlayAfterFrame(2, soundIndex));
	}

	// USE TO PLAY CHARACTER SFX
	public void PlayCharacterSound(int soundIndex)
	{
		this.StartCoroutine(this.PlayAfterFrame(3, soundIndex));
	}


	public void PlayMusic(int index)
    {
        this.StartCoroutine(this.PlayMusicAfterFrame(index));
		//Debug.Log(!GameManager.Instance.MusicVolume);
    }

	public void StopMusic(int index)
	{
		MusicAudioSources [index].StopLoopingMusicManaged();
	}

	public void PauseAllSounds()
    {
		SoundManager.PauseAll();
    }

	protected IEnumerator PlayAfterFrame(int soundTypeIndex, int soundIndex)
    {
        yield return new WaitForEndOfFrame();
        AudioSource aux = AllSFXSources[soundTypeIndex][soundIndex];
        aux.PlayOneShotSoundManaged(aux.clip);
    }

	protected IEnumerator PlayMusicAfterFrame(int soundIndex)
    {
        yield return new WaitForEndOfFrame();
		MusicAudioSources [soundIndex].PlayLoopingMusicManaged(SoundManager.MusicVolume, 1.0f, true);


    }
}
