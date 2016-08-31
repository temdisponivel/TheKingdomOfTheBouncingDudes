using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;


namespace BounceDudes{

	// This scene was created because there were some issues in the SplashScreen being first. Don't ask, just go along :D

	public class BlankAndFirstScreen : MonoBehaviour {


		public GameObject _loadImage;
		public Vector3 _scaleFinalValue;

		private AsyncOperation _async = null;

		// Use this for initialization
		void Start () {

			this.StartCoroutine (this.WaitForAndCall (0.5f, () => {
				_async = SceneManager.LoadSceneAsync ("TrueSplashScreen");
				LoadAsyncLevel();
			}));

		}

		private IEnumerator LoadAsyncLevel(){
			yield return _async;
		}

		private IEnumerator WaitForAndCall(float seconds, Action callback)
		{

			yield return new WaitForSeconds(seconds);
			if (callback != null)
				callback();
		}

		// Update is called once per frame
		void Update () {
			if (_async != null) {
				
				var newScale = new Vector3(_scaleFinalValue.x * _async.progress, _scaleFinalValue.y * _async.progress, _scaleFinalValue.z * _async.progress);
				_loadImage.transform.DOScale (newScale, 0.1f);

			}
		}
	}
}