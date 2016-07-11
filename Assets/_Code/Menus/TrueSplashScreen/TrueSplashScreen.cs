using UnityEngine;
using System.Collections;
using DG.Tweening;


namespace BounceDudes
{

	public class TrueSplashScreen : MonoBehaviour {

		public RectTransform Logo;
		public GameObject FinalLogoPos;

		// Use this for initialization
		void Start () {

			Logo.DOMove (FinalLogoPos.transform.position, 3.0f).OnComplete(() => { 
				GameManager.Instance.LoadScene("TitleScreen"); 
			});
		}

	}
}