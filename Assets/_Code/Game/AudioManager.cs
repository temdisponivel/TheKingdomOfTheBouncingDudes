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
            SoundManager.SoundVolume = 0;

        if (!GameManager.Instance.MusicVolume)
            SoundManager.MusicVolume = 0;
    }

    public void ToggleMusicVolume()
    {
        if (GameManager.Instance.MusicVolume)
            SoundManager.MusicVolume = _musicVolume;
        else
            SoundManager.MusicVolume = 0;
        //SoundManager.MusicVolume = SoundManager.MusicVolume == 0 ? _musicVolume : 0;
    }

    public void ToggleSoundVolume()
    {
        if (GameManager.Instance.SoundVolume)
            SoundManager.SoundVolume = _soundVolume;
        else
            SoundManager.SoundVolume = 0;

        //SoundManager.SoundVolume = SoundManager.SoundVolume == 0 ? _soundVolume : 0;
        //GameManager.Instance.SoundVolume = SoundManager.SoundVolume != 0;
    }

    public void PlaySound(int soundTypeIndex, int soundIndex)
    {
        //AudioSource aux = AllSFXSources[soundTypeIndex][soundIndex];
        //aux.PlayOneShotSoundManaged(aux.clip);
        this.StartCoroutine(this.PlayAfterFrame(soundTypeIndex, soundIndex));
    }

    public void PlayInterfaceSound(int soundIndex)
    {
        //AudioSource aux = AllSFXSources[0][soundIndex];
        //aux.PlayOneShotSoundManaged(aux.clip);
        this.StartCoroutine(this.PlayAfterFrame(0, soundIndex));
    }

    public void PlayMusic(int index)
    {
        //MusicAudioSources[index].PlayLoopingMusicManaged(1.0f, 1.0f, true);
        this.StartCoroutine(this.PlayMusicAfterFrame(index));
    }

    public void StopCurrentAudio()
    {
        SoundManager.StopAll();
    }

    public IEnumerator PlayAfterFrame(int soundTypeIndex, int soundIndex)
    {
        yield return new WaitForEndOfFrame();
        AudioSource aux = AllSFXSources[soundTypeIndex][soundIndex];
        aux.PlayOneShotSoundManaged(aux.clip);
    }

    public IEnumerator PlayMusicAfterFrame(int soundIndex)
    {
        yield return new WaitForEndOfFrame();
        MusicAudioSources[soundIndex].PlayLoopingMusicManaged(1.0f, 1.0f, true);
    }
}