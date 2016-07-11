using UnityEngine;
using System.Collections;
using DG.Tweening;


namespace BounceDudes
{
	public class TrueSplashScreen : MonoBehaviour {

		public RectTransform Logo;
		public GameObject MiddleLogoPos;
		public GameObject FinalLogoPos;

		// Use this for initialization
		void Start () {

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
				
		}

	}
}