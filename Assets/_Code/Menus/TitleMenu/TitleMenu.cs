﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace BounceDudes
{

    /// <summary>
    /// Class that controls the Title Menu.
    /// </summary>
    public class TitleMenu : MonoBehaviour
    {

        public ParticleSystem _dustEffect;
        public Transform _kingHandPoint;

        public Animator _mainMenuAnimator;
        public GameObject _mainMenuContainer;

        public Animator _balloonAnimator;

        protected Animator _animator;

        protected bool _animating = false;

        public GameObject ConfigPanel;

        public GameObject ConfigShownPosition;
        public GameObject ConfigHiddenPosition;

        public int _currentState = 0;
        protected const int ON_SPLASH_SCREEN = 0;
        protected const int ON_MAIN_MENU = 1;
        

        public void Start()
        {
            this._animator = this.GetComponent<Animator>();
        }


        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (this._currentState == ON_SPLASH_SCREEN)
                {
                    this._animator.SetTrigger("GoSplashToMain");
                }

            }
        }

        // --- To be called in the end of animations
        public void ChangeStateToMainMenu()
        {
            _currentState = ON_MAIN_MENU;
            _mainMenuAnimator.ResetTrigger("CallIntro");

        }

        public void ChangeStateToSplashScreen()
        {
            _currentState = ON_SPLASH_SCREEN;
            _mainMenuAnimator.ResetTrigger("CallOutro");
        }
        
        public void CallCapeBlown()
        {
            this._animator.SetTrigger("KingCapeBlown");
        }

        public void CallDustParticle()
        {
            Instantiate(_dustEffect, _kingHandPoint.position, _kingHandPoint.rotation);
        }

        public void CallChainsIntro()
        {
            _balloonAnimator.SetTrigger("CallOutro");
            _mainMenuAnimator.SetTrigger("CallIntro");
        }

        public void CallChainsOutro()
        {
            _mainMenuAnimator.SetTrigger("CallOutro");
        }

        public void Play()
        {
            SceneManager.LoadScene("MapMenu");
        }

        public void Troop()
        {
            SceneManager.LoadScene("TroopMenu");
        }

        public void ShowSetting()
        {
            this.ConfigPanel.transform.DOMove(this.ConfigShownPosition.transform.position, 1);
        }

        public void HideSetting()
        {
            this.ConfigPanel.transform.DOMove(this.ConfigHiddenPosition.transform.position, 1);
        }

        public void Music(bool mute)
        {
            Debug.Log(mute);
            GameManager.Instance.MusicVolume = mute ? 0 : 1;
        }

        public void Sound(bool mute)
        {
            Debug.Log(mute);
            GameManager.Instance.SoundVolume = mute ? 0 : 1;
        }
    }

}