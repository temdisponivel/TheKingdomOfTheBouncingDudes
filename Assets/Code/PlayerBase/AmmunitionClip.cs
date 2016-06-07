using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        public GameObject _launcherObject;
        public GameObject _changeToFieldOrderPoint;

        protected bool _isOutOfAmmo = true;
        protected List<Soldier> _ammunitionClip = new List<Soldier>();

        protected int _positions = 0;
        protected int _shootPosition = 0;

        public bool IsOutOfAmmo { get { return _isOutOfAmmo; } }
        public Soldier NextAmmunition { get { return _ammunitionClip[_shootPosition % _ammunitionClip.Count].GetComponent<Soldier>(); } }

        public List<Soldier> SoldiersInClip { get { return _ammunitionClip; } }

        public event Action AmmoCountChanged;


        // Use this for initialization
        void Start()
        {
            AmmunitionClip.Instance = this;
            this.FillAmmunitionClip();
            this.PrepareNextAmmunition();
        }

        protected void FillAmmunitionClip()
        {
            List<GameObject> formationList = GameManager.Instance.NextLevelSoldier;
			//List<GameObject> formationList = GameManager.Instance.GetAvailableSoldiers(); // FOR TESTS ONLY

            foreach (GameObject ammo in formationList)
            {
                this.AddAmmunition(ammo);
            }

            GameManager.Instance.NextLevelSoldier.Clear();
        }
        
        public void AddAmmunition(GameObject ammunition)
        {
            Transform ammoNewPoint = GetAmmunitionPositionOnWorld(-1); // Position out of camera bounds
            GameObject ammoInstance = (GameObject)GameObject.Instantiate(ammunition, ammoNewPoint.position, ammoNewPoint.rotation);

            // change the new instance of ammunition
            Soldier ammoSoldier = ammoInstance.GetComponent<Soldier>();
            ammoSoldier.OriginalGameObject = ammunition;

            this._ammunitionClip.Add(ammoSoldier);

            this.InnerAddAmmunition(ammoInstance);
        }

        public void RecycleAmmunition(GameObject ammoInstance)
        {
            //ammoInstance.transform.position = ammoNewPoint.position;
            //ammoInstance.transform.rotation = ammoNewPoint.rotation;

            this.InnerAddAmmunition(ammoInstance);
        }

        protected void InnerAddAmmunition(GameObject ammoInstance)
        {
            // change the new instance of ammunition
            Soldier ammoSoldier = ammoInstance.GetComponent<Soldier>();
            ammoSoldier.AmmunitionPosition = _positions++ % this._ammunitionClip.Count;
            ammoSoldier.StartMoveAnimation(this.GetAmmunitionPositionOnWorld(ammoSoldier.AmmunitionPosition).position, 5.0f);

            //this._ammunitionClip.Add(ammoInstance);
            this._isOutOfAmmo = false; // Put this when ammo animation ends
            if (this.AmmoCountChanged != null)
                this.AmmoCountChanged();
        }

        public void ShootNextAmmunition()
        {
            if (this._ammunitionClip.Count == 0)
            {
                // -- call action when out of Ammo --
                this._isOutOfAmmo = true;
                return;
            }

            NextAmmunition.Shoot();

            _shootPosition++;

            //this._ammunitionClip.RemoveAt(0);

            foreach (Soldier ammo in this._ammunitionClip)
            {
                ammo.AmmunitionPosition = ++ammo.AmmunitionPosition % this._ammunitionClip.Count;

                ammo.StartMoveAnimation(this.GetAmmunitionPositionOnWorld(ammo.AmmunitionPosition).position, 4.0f);
            }

        }

        public void PrepareNextAmmunition()
        {
            // --- call this in the Reload Animation
            NextAmmunition.CurrentSortingOrder = NextAmmunition.SpriteOrderOnBarrel;
            NextAmmunition.StartMoveAnimation(this._onBarrelPoint.transform.position, 5.0f);

            this.NextAmmunition.transform.rotation = Weapon.Instance.WeaponRotation;
            this.NextAmmunition.transform.SetParent(this._launcherObject.transform);
        }

        public Transform GetAmmunitionPositionOnWorld(int indexInList)
        {

            Transform aux;

            switch (indexInList)
            {
                case 2:
                    aux = _nextPoint.transform;
                    break;
                case 1:
                    aux = _secondPoint.transform;
                    break;
                case 0:
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