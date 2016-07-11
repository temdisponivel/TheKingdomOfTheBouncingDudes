using UnityEngine;
using System.Collections;
using DG.Tweening;
using DigitalRuby.SoundManagerNamespace;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        public Toggle ToggleSoundUI;
        public Toggle ToggleMusicUI;

        protected bool _animating = true;

        public GameObject ConfigPanel;

        public GameObject _shade;
        public GameObject ConfigShownPosition;
        public GameObject ConfigHiddenPosition;

        public int _currentState = 0;
        protected const int ON_SPLASH_SCREEN = 0;
        protected const int ON_MAIN_MENU = 1;
		protected const int ON_SETTINGS = 2;

        public void Start()
        {
			GameManager.Instance.CreateFullSave (true); // FOR TESTS ONLY

            this._animator = this.GetComponent<Animator>();

            if (GameManager.Instance.PassSplashScreen)
            {
                this.PlayInterfaceSound(7);
				_balloonAnimator.SetTrigger("CallOutro");
                this._animator.SetTrigger("GoSplashToMain");
            }
            else
            {
                AudioManager.Instance.PlayMusic(0);
				_animating = false;
            }

            ToggleMusicUI.isOn = GameManager.Instance.MusicVolume;
            ToggleSoundUI.isOn = GameManager.Instance.SoundVolume;
        }


        public void Update()
        {
			if (Input.GetMouseButton(0) && !_animating)
            {
                if (this._currentState == ON_SPLASH_SCREEN)
                {
                    this._animator.SetTrigger("GoSplashToMain");
                    this.PlayInterfaceSound(7);
                    AudioManager.Instance.PlayMusic(1);
                }

            }

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (this._currentState == ON_SETTINGS) {
					this.HideSetting ();
				}
			}

        }

        // --- To be called in the end of animations
        public void ChangeStateToMainMenu()
        {
            _currentState = ON_MAIN_MENU;
            _mainMenuAnimator.ResetTrigger("CallIntro");
            GameManager.Instance.PassSplashScreen = true;
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
			GameManager.Instance.FromMainMenuToMapMenu = true;
            GameManager.Instance.LoadScene("MapMenu");
			this.PlayInterfaceSound(2);
            //SceneManager.LoadScene("MapMenu");
        }

		public void ShowAchievements(){
			this.PlayInterfaceSound(0);
			GameManager.GPManagerInstance.CallAchievementsUI ();
		}

        public void Credits()
        {
            GameManager.Instance.LoadScene("CreditsMenu");
			this.PlayInterfaceSound(0);
            //SceneManager.LoadScene("TroopMenu");
        }

        public void ShowSetting()
        {
			_currentState = ON_SETTINGS;
            this._shade.transform.DOScale(1, 0.2f);
            this.ConfigPanel.transform.DOMove(this.ConfigShownPosition.transform.position, 0.5f);
			this.PlayInterfaceSound(0);
        }

        public void HideSetting()
        {
			_currentState = ON_MAIN_MENU;
            this._shade.transform.DOScale(0, 0.2f);
            this.ConfigPanel.transform.DOMove(this.ConfigHiddenPosition.transform.position, 0.5f);
			this.PlayInterfaceSound(0);

        }

        public void ToggleMusic(bool on)
        {
            GameManager.Instance.MusicVolume = on;
            AudioManager.Instance.ToggleMusicVolume();
			this.PlayInterfaceSound(0);
        }

        public void ToggleSound(bool on)
        {
            GameManager.Instance.SoundVolume = on;
            AudioManager.Instance.ToggleSoundVolume();
			this.PlayInterfaceSound(0);
        }

        public void PlayInterfaceSound(int index)
        {
            AudioManager.Instance.PlayInterfaceSound(index);
        }
    }

}