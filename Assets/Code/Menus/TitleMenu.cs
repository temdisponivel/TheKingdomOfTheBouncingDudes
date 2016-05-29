using UnityEngine;
using System.Collections;

namespace BounceDudes{

	/// <summary>
	/// Class that controls the Title Menu.
	/// </summary>
	public class TitleMenu : MonoBehaviour {

		public ParticleSystem _dustEffect;
		public Transform _kingHandPoint;

		public Animator _mainMenuAnimator;
		public GameObject _mainMenuContainer;

		protected Animator _animator;


		public int _currentState = 0;
		protected const int ON_SPLASH_SCREEN = 0;
		protected const int ON_MAIN_MENU = 1;


		public void Start(){
			this._animator = this.GetComponent<Animator> ();
		}


		public void Update(){
			if (Input.GetMouseButton (0)) {
				if (this._currentState == ON_SPLASH_SCREEN){
					this._animator.SetTrigger ("GoSplashToMain");
				}
				else if (this._currentState == ON_MAIN_MENU){
					this._animator.SetTrigger ("GoMainToSplash");
				}

			}
		}


		public void CallCapeBlown(){
			this._animator.SetTrigger ("KingCapeBlown");
		}

		public void CallDustParticle(){
			Instantiate(_dustEffect, _kingHandPoint.position, _kingHandPoint.rotation);
		}

		public void CallChainsIntro(){
			_mainMenuAnimator.SetTrigger ("CallIntro");
		}

		public void CallChainsOutro(){
			_mainMenuAnimator.SetTrigger ("CallOutro");
		}
	
	}

}