using UnityEngine;
using System.Collections;
using Assets._Code.Game;

namespace BounceDudes{

	public class ArenaMenu : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void Return()
		{
			AudioManager.Instance.PlayInterfaceSound (0);
			GameManager.Instance.LoadScene("TitleScreen");
		}


		public void Formation(){
			AudioManager.Instance.PlayInterfaceSound (0);
			GameManager.Instance.CurrentLevel = GameManager.Instance.LevelsById[LevelId.ARENA];
			GameManager.Instance.LoadScene("FormationMenu");
		}

		public void Troop()
		{
			AudioManager.Instance.PlayInterfaceSound (0);
			GameManager.Instance.LoadScene("TroopMenu");
		}
	}
}
