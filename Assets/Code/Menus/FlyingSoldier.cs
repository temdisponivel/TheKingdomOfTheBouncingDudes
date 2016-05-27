using UnityEngine;
using System.Collections;

namespace BounceDudes{

	public class FlyingSoldier : MonoBehaviour {

		public GameObject _flyingSoldier;
		public GameObject[] _soldierArray;


		public void ChangeSoldierSprite(){

			Sprite flyingSoldierSprite = _flyingSoldier.GetComponent<SpriteRenderer>().sprite;
			Sprite randomSoldierSprite;

			// Pick a random soldier from the array and make it not repeat.
			do {
				randomSoldierSprite = (_soldierArray [Random.Range (0, _soldierArray.Length)]).GetComponent<SpriteRenderer>().sprite;
			} while (randomSoldierSprite == flyingSoldierSprite);

			_flyingSoldier.GetComponent<SpriteRenderer>().sprite = randomSoldierSprite;
			_flyingSoldier


		}

	}
}