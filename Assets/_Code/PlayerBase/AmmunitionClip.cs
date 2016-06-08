using System;
using UnityEngine;
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

        public bool IsOutOfAmmo { get { return this._ammunitionClip.Count == 0; } }
        public Soldier NextAmmunition { get { return _ammunitionClip.Count > 0 ? _ammunitionClip.Peek() : null; } }

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
            GameManager.Instance.NextLevelSoldiers.Clear();
            this.FillAmmunitionClip();
        }

        protected void FillAmmunitionClip()
        {
            foreach (GameObject ammo in SoldiersBkp)
            {
                this.AddAmmunition((GameObject)GameObject.Instantiate(ammo, this._othersPoint.transform.position, this._othersPoint.transform.rotation), ammo);
            }
        }

        public void AddAmmunition(GameObject ammunition, GameObject original = null, bool recycleAnimation = false)
        {
            Soldier ammoSoldier = ammunition.GetComponent<Soldier>();
            ammoSoldier.OriginalGameObject = original;
            ammoSoldier.AmmunitionPosition = this._ammunitionClip.Count;

            this._ammunitionClip.Enqueue(ammoSoldier);

			if (!recycleAnimation)
				ammoSoldier.CallMoveToAnimation (this.GetAmmunitionPositionOnWorld (ammoSoldier.AmmunitionPosition).position);
			else
				ammoSoldier.CallFlyToAnimation (this.GetAmmunitionPositionOnWorld (ammoSoldier.AmmunitionPosition).position);

            if (this.ShootedSoldiers.Contains(ammoSoldier))
                this.ShootedSoldiers.Remove(ammoSoldier);

            if (this.AmmoCountChanged != null)
                this.AmmoCountChanged();
        }

        public void ShootNextAmmunition()
        {
            if (this.IsOutOfAmmo)
                return;

            NextAmmunition.Shoot();

            this.ShootedSoldiers.Add(_ammunitionClip.Dequeue());

            foreach (Soldier ammo in this._ammunitionClip)
            {
				ammo.transform.DOMove(this.GetAmmunitionPositionOnWorld(--ammo.AmmunitionPosition).position, ammo.TimeToTravel / 2);
            }
        }

        public void PrepareNextAmmunition()
        {
            if (this.IsOutOfAmmo)
                return;

			this.NextAmmunition.GetComponent<Soldier> ().OnBarrel();

			NextAmmunition.transform.position = this._onBarrelPoint.transform.position;

            this.NextAmmunition.transform.rotation = Weapon.Instance.WeaponRotation;
			this.NextAmmunition.transform.SetParent(this._shootObject.transform);
			// Ensures that the shoot object will move with the platform. A way around parenting and messing up the scale of the projectile.
			this._shootObject.GetComponent<ShootObject> ().ObjectToFollow = this._onBarrelPoint;

			//NextAmmunition.transform.DOMove(this._onBarrelPoint.transform.position, NextAmmunition._timeToTravel * 2);
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