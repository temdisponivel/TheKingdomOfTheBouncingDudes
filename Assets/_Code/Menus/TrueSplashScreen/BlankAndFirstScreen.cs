using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BounceDudes{

	// This scene was created because there were some issues in the SplashScreen being first. Don't ask, just go along :D

	public class BlankAndFirstScreen : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
			SceneManager.LoadScene ("TrueSplashScreen");
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}