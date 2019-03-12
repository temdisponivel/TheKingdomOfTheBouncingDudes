using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


namespace BounceDudes
{
	public class TrueSplashScreen : MonoBehaviour {

		public RectTransform Logo;
		public GameObject MiddleLogoPos;
		public GameObject FinalLogoPos;
		public Text DescriptionText;


		void Awake(){
			
		}

		// Use this for initialization
		void Start () {
			
			#if UNITY_ANDROID
			GameManager.GPManagerInstance.AuthenticateUser ();
			#endif

			DescriptionText.GetComponent<TextToTraslate> ().Translate();

			this.StartCoroutine (this.WaitForAndCall (0.3f, () => {
				// I should use Sequences, but argh, I didn't understood them :(
				Logo.DOMoveY (Logo.position.y - 0.5f, 0.7f).OnComplete (() => {
					Logo.DOMove (MiddleLogoPos.transform.position, 1.5f).OnComplete(() => {
						Logo.DOMoveY(Logo.position.y - 0.5f, 0.7f).OnComplete(() => { 
							Logo.DOMove (FinalLogoPos.transform.position, 2.0f).OnComplete(() => {
								GameManager.Instance.LoadScene("TitleScreen");
							});  
						});
					});
				});
			}));


				
		}

		public IEnumerator WaitForAndCall(float seconds, Action callback)
		{

			yield return new WaitForSeconds(seconds);
			if (callback != null)
				callback();
		}

	}
}