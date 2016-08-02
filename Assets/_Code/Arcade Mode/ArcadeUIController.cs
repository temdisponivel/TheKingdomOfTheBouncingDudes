using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BounceDudes{

	public class ArcadeUIController : MonoBehaviour {

		public static ArcadeUIController Instance;


		[Header("UI")]
		public Text _arcadeScoreText;

		// Use this for initialization
		void Awake () {
			Instance = this;
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void UpdateScoreText(int value){
			this._arcadeScoreText.text = value.ToString();
		}
	}
}
