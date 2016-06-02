using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BounceDudes{

	public class AmmunitionClip : MonoBehaviour {

		static public AmmunitionClip Instance = null;

		[Header("Ammunitions Points")]
		public GameObject _onBarrelPoint;
		public GameObject _secondPoint;
		public GameObject _thirdPoint;
		public GameObject _othersPoint;
		public GameObject _changeToProjectilePoint;

		protected bool _isOutOfAmmo = true;
		protected List<GameObject> _ammunitionClip = new List<GameObject> ();

		public bool IsOutOfAmmo { get { return _isOutOfAmmo; } }
		public GameObject NextAmmunition { get { return _ammunitionClip[0]; } }


		// Use this for initialization
		void Start () {
			
			AmmunitionClip.Instance = this;
			this.FillAmmunitionClip();

		}

		protected void FillAmmunitionClip(){

			List<GameObject> formationList = GameManager.Instance.GetAvailableSoldiers (); // TEMPORARY. Will be players input

			foreach (GameObject ammo in formationList){
				this.AddAmmunition (ammo);
			}
		}
			

		public void AddAmmunition(GameObject ammunition){

			Transform newPoint = GetAmmunitionPositionOnWorld (this._ammunitionClip.Count);
			GameObject ammoInstance = (GameObject)GameObject.Instantiate(ammunition, newPoint.position, newPoint.rotation);

			// Change the new instance
			Soldier ammoSoldier = ammoInstance.GetComponent<Soldier> ();
			ammoSoldier.OriginalGameObject = ammunition;
			ammoSoldier.AmmunitionPosition = this._ammunitionClip.Count;
			ammoSoldier.Sprite.sortingOrder = 16;	


			this._ammunitionClip.Add (ammoInstance);
			// --- Call Animation to arriving position here ---
			this._isOutOfAmmo = false; // Put this when ammo animation ends
		}

		public void ShootNextAmmunition(){

			if (this._ammunitionClip.Count == 0) {
				// -- Call action when out of Ammo --
				this._isOutOfAmmo = true;
				return;
			}

			NextAmmunition.GetComponent<Soldier>().Shoot();

			this._ammunitionClip.RemoveAt (0);

			foreach (GameObject ammo in this._ammunitionClip) {
				Soldier ammoSoldier = ammo.GetComponent<Soldier> ();
				// --- Call Animation to change position here --
				ammoSoldier.AmmunitionPosition--;
				ammo.transform.position = this.GetAmmunitionPositionOnWorld (ammoSoldier.AmmunitionPosition).position;

			}

		}

		public Transform GetAmmunitionPositionOnWorld(int indexInList){
			
			Transform aux; 

			switch (indexInList) {
			case 0:
				aux = _onBarrelPoint.transform;
				break;
			case 1:
				aux = _secondPoint.transform;
				break;
			case 2:
				aux = _thirdPoint.transform;
				break;
			default:
				aux = _othersPoint.transform;
				break;
			}

			return aux;
		}
			
	}

}