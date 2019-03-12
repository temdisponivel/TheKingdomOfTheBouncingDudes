using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace BounceDudes
{

    public class AmmunitionClip : MonoBehaviour
    {

        static public AmmunitionClip Instance = null;

        [Header("Ammunitions Points")]
        public GameObject _nextPoint;
        public GameObject _secondPoint;
        public GameObject _thirdPoint;
        public GameObject _othersPoint;
        public GameObject _changeToProjectilePoint;
        public GameObject _onBarrelPoint;
		public GameObject _shootObject;
		public GameObject _changeToFieldOrderPoint;

        protected Queue<Soldier> _ammunitionClip = new Queue<Soldier>();
        protected List<GameObject> SoldiersBkp;

        protected int _positions = 0;
		protected int _ammoClipCount = 0;

		public bool IsOutOfAmmo { get { return _ammoClipCount == 0; } }
		public Soldier NextAmmunition { get { return _ammoClipCount > 0 ? _ammunitionClip.Peek() : null; } }

        public Queue<Soldier> SoldiersInClip { get { return _ammunitionClip; } }

        public List<Soldier> ShootedSoldiers = new List<Soldier>();

        public event Action AmmoCountChanged;

        public void Awake()
        {
            AmmunitionClip.Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            SoldiersBkp = GameManager.Instance.NextLevelSoldiers.ToList();
			//SoldiersBkp = GameManager.Instance.GetAvailableSoldiers();
            this.FillAmmunitionClip();
        }

        protected void FillAmmunitionClip()
        {
            foreach (GameObject ammo in SoldiersBkp)
            {
				this.AddAmmunition (ammo);

            }
				
        }

        public void AddAmmunition(GameObject ammunition)
        {
            Soldier ammoSoldier = ammunition.GetComponent<Soldier>();
          
			this.ShootedSoldiers.Remove(ammoSoldier);

            ammoSoldier.RigidBody.angularVelocity = 0f;
            ammoSoldier.RigidBody.velocity = Vector2.zero;
            
			ammoSoldier.transform.DOMove (this._othersPoint.transform.position, ammoSoldier.TimeToTravel / 4).OnComplete (() => {

				ammoSoldier.AmmunitionPosition = this._ammoClipCount;
				this._ammunitionClip.Enqueue(ammoSoldier);
				_ammoClipCount++;

				ammoSoldier.transform.DOMove(this.GetAmmunitionPositionOnWorld(ammoSoldier.AmmunitionPosition).position, ammoSoldier.TimeToTravel / 8).OnComplete(() => {

					if (this.AmmoCountChanged != null)
						this.AmmoCountChanged();

				}).SetId("Transform"); 

			}).SetId("Transform");
				
        }


        public void ShootNextAmmunition()
        {
            if (this.IsOutOfAmmo)
                return;

            Soldier shooted;
            this.ShootedSoldiers.Add(shooted = _ammunitionClip.Dequeue());
			_ammoClipCount--;
           
            shooted.Shoot();

            foreach (Soldier ammo in this._ammunitionClip)
            {
				ammo.transform.DOMove(this.GetAmmunitionPositionOnWorld(--ammo.AmmunitionPosition).position, ammo.TimeToTravel / 2);
            }
        }

        public void PrepareNextAmmunition()
        {
            if (this.IsOutOfAmmo)
                return;

			this.NextAmmunition.GetComponent<Soldier>().OnBarrel();

			NextAmmunition.transform.position = this._onBarrelPoint.transform.position;
        }

        public Transform GetAmmunitionPositionOnWorld(int indexInList)
        {
            Transform aux;
            switch (indexInList)
            {
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