using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BounceDudes{

	public class AmmunitionClip : MonoBehaviour {

		static public AmmunitionClip Instance = null;

		[Header("Ammunitions Points")]
		public GameObject _nextPoint;
		public GameObject _secondPoint;
		public GameObject _thirdPoint;
		public GameObject _othersPoint;
		public GameObject _changeToProjectilePoint;
		public GameObject _onBarrelPoint;
		public GameObject _launcherObject;
		public GameObject _changeToFieldOrderPoint;

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

			List<GameObject> formationList = GameManager.Instance.GetAvailableSoldiers (); //TODO: TEMPORARY. Will be player input

			foreach (GameObject ammo in formationList){
				this.AddAmmunition (ammo);
			}
		}
			

		public void AddAmmunition(GameObject ammunition){

			Transform ammoNewPoint = GetAmmunitionPositionOnWorld (-1); // Position out of bounds
			GameObject ammoInstance = (GameObject)GameObject.Instantiate(ammunition, ammoNewPoint.position, ammoNewPoint.rotation);

			// change the new instance of ammunition
			Soldier ammoSoldier = ammoInstance.GetComponent<Soldier> ();
			ammoSoldier.OriginalGameObject = ammunition;
			ammoSoldier.AmmunitionPosition = this._ammunitionClip.Count;
			ammoSoldier.StartMoveAnimation (this.GetAmmunitionPositionOnWorld (ammoSoldier.AmmunitionPosition).position, 5.0f);
	
			this._ammunitionClip.Add (ammoInstance);
			this._isOutOfAmmo = false; // Put this when ammo animation ends
		}

		public void ShootNextAmmunition(){

			if (this._ammunitionClip.Count == 0) {
				// -- call action when out of Ammo --
				this._isOutOfAmmo = true;
				return;
			}

			NextAmmunition.GetComponent<Soldier>().Shoot();

			this._ammunitionClip.RemoveAt (0);

			foreach (GameObject ammo in this._ammunitionClip) {
				Soldier ammoSoldier = ammo.GetComponent<Soldier> ();
				ammoSoldier.AmmunitionPosition--;

				ammoSoldier.StartMoveAnimation (this.GetAmmunitionPositionOnWorld (ammoSoldier.AmmunitionPosition).position, 4.0f);

			}

		}

		public void PrepareNextAmmunition(){
			// --- call this in the Reload Animation
			Soldier ammoSoldier = NextAmmunition.GetComponent<Soldier> ();
			ammoSoldier.CurrentSortingOrder = ammoSoldier.SpriteOrderOnBarrel;
			ammoSoldier.StartMoveAnimation (this._onBarrelPoint.transform.position, 5.0f);

			this.NextAmmunition.transform.rotation = Weapon.Instance.WeaponRotation;
			this.NextAmmunition.transform.parent = this._launcherObject.transform;

		}

		public Transform GetAmmunitionPositionOnWorld(int indexInList){
			
			Transform aux; 

			switch (indexInList) {
			case 0:
				aux = _nextPoint.transform;
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