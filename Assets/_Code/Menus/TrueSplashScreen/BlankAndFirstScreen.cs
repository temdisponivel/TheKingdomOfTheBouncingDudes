using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


namespace BounceDudes{

	// This scene was created because there were some issues in the SplashScreen being first. Don't ask, just go along :D

	public class BlankAndFirstScreen : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
			this.StartCoroutine (this.WaitForAndCall (0.5f, () => {
				SceneManager.LoadSceneAsync ("TrueSplashScreen");
			}));

		}


		public IEnumerator WaitForAndCall(float seconds, Action callback)
		{

			yield return new WaitForSeconds(seconds);
			if (callback != null)
				callback();
		}

		// Update is called once per frame
		void Update () {
		
		}
	}
}