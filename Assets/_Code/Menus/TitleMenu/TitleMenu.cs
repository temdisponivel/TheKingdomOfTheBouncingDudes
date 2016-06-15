using UnityEngine;
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

		public GameObject _shade;
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
            GameManager.Instance.LoadScene("MapMenu");
            //SceneManager.LoadScene("MapMenu");
        }

        public void Troop()
        {
            GameManager.Instance.LoadScene("TroopMenu");
            //SceneManager.LoadScene("TroopMenu");
        }

        public void ShowSetting()
        {
			this._shade.transform.DOScale (1, 0.2f);
            this.ConfigPanel.transform.DOMove(this.ConfigShownPosition.transform.position, 0.5f);
        }

        public void HideSetting()
        {
			this._shade.transform.DOScale (0, 0.2f);
            this.ConfigPanel.transform.DOMove(this.ConfigHiddenPosition.transform.position, 0.5f);
        }

        public void Music(bool mute)
        {
            GameManager.Instance.MusicVolume = !mute ? 0 : 1;
        }

        public void Sound(bool mute)
        {
            GameManager.Instance.SoundVolume = !mute ? 0 : 1;
        }
    }

}