﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BounceDudes
{
    /// <summary>
    /// Weapon controlled by player.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        static public Weapon Instance = null;

        public Text _textAngle = null;

        public Camera _camera = null;
        public Camera _cameraUi = null;

        protected List<GameObject> _projectiles = new List<GameObject>();
        protected List<GameObject> _projectilesSpecial = new List<GameObject>();

        public float _minShootMultiplier = 0.45f;
        public float _maxShootMultiplier = 0.8f;
        public float _coolDown = 0f;
        public float _shootMultiplierPerSeconds = 2;

        [Tooltip("Max angle in relation to right. When pointing at right, the angle is zero; when pointing to left is 180.")]
        public float _maxRotationAngle;

        [Tooltip("Min angle in relation to right. When pointing at right, the angle is zero; when pointing to left is 180.")]
        public float _minRotationAngle;

        public bool _canShoot = true;


        [Header("Objects")]
        public GameObject _idlePoint = null;
        public GameObject _specialShootPoint = null;
        public GameObject _minorCogObject = null;


        [Header("Visual settings")]
        public int _quantityOfShownMunition = 10;


        [Header("Special")]
        public float _coolDownBetweenSpecials = 3f;
        public float _specialDuration = 3f;
        protected bool _special = false;
        protected float _specialStartTime = 0;

        protected float _lastTimeShoot = 0f;
        protected float _currentForceMultiplier = 0f;
        protected bool _holding = false;
        protected int _currentProjectileIndex = 0;
        protected int _currentSpecialProjectileIndex = 0;

        protected Animator _weaponAnimator = null;
        private bool _waitingForAmmo;

        public int ShootCount { get; set; }
        public float ForceMultiplier { get { return this._currentForceMultiplier; } }
        public Quaternion WeaponRotation { get { return this.transform.rotation; } }

        public void Awake()
        {
            Weapon.Instance = this;
            this._weaponAnimator = this.GetComponent<Animator>();
        }

        public void Start()
        {
            AmmunitionClip.Instance.AmmoCountChanged += this.NewAmmo;
            this._projectiles = GameManager.Instance.GetAvailableSoldiers();
            this._projectilesSpecial = GameManager.Instance._specialProjectiles;

			this.CallReloadAnimation ();
        }

        public void Update()
        {
            this.ShootRoutine();
            
            this._special = this._special && (Time.time - this._specialStartTime) <= this._specialDuration;
        }

        public void SetSpecial()
        {
            this._special = (Time.time - (this._specialStartTime + this._specialDuration)) >= this._coolDownBetweenSpecials;
            this._specialStartTime = Time.time;
        }

        /// <summary>
        /// Perform the logic of shooting.
        /// </summary>
        protected void ShootRoutine()
        {
            bool clicked = Input.GetMouseButton(0);

            if (clicked)
            {
                var hit = Physics2D.Raycast(this._cameraUi.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI"));

                if (hit.collider != null)
                {
                    return;
                }

                if (AmmunitionClip.Instance.IsOutOfAmmo)
                    this._waitingForAmmo = true;

                this.RotateTowardsMouse();

                if (this._waitingForAmmo)
                    return;

                if (!this._canShoot)
                    return;

                if (!this._holding)
                {
                    this._holding = true;
                    this._currentForceMultiplier = this._minShootMultiplier;
                    this._weaponAnimator.SetTrigger("Holding");
                }

                if (this._currentForceMultiplier < this._maxShootMultiplier)
                {
                    this._currentForceMultiplier += this._shootMultiplierPerSeconds * Time.deltaTime;
                }

				if (this._currentForceMultiplier >= this._maxShootMultiplier)
					this._weaponAnimator.SetFloat ("Speed", 1);
				else
					this._weaponAnimator.SetFloat ("Speed", 1 / (this._maxShootMultiplier / this._shootMultiplierPerSeconds));
            }
            else if (this._holding)
            {
                if (!this._canShoot)
                    return;

                if (this._special)
                {
                    this._currentForceMultiplier = this._maxShootMultiplier;
                    this.ShootSpecial();
                }
                else if (Time.time - this._lastTimeShoot >= this._coolDown)
                {
                    this._weaponAnimator.SetTrigger("Shooting"); // Calls Shoot() on the animation clip.
                }
                this._holding = false;
            }
        }

        public void CallReloadAnimation()
        {
            this._weaponAnimator.SetTrigger("Reloading"); // Calls PrepareAmmunition() in mid animation
        }

        //called from animator
        public void SetCanShoot()
        {
            this._canShoot = true;
        }

        //called from animator
        public void UnSetCanShoot()
        {
            this._canShoot = false;
        }

        public void CallPrepareAmmunition()
        {
            AmmunitionClip.Instance.PrepareNextAmmunition();
            this.UpdateSoldierNameUI();
        }

        /// <summary>
        /// Shoot the current projectile.
        /// </summary>
        public void Shoot()
        {
            AmmunitionClip.Instance.ShootNextAmmunition();
            this._lastTimeShoot = Time.time;
            this.ShootCount++;
        }

        /// <summary>
        /// Shoot the special projectiles.
        /// </summary>
        public void ShootSpecial()
        {
            this.ShootSpecialObject(this._projectilesSpecial[this._currentSpecialProjectileIndex]);
            this._currentSpecialProjectileIndex = (this._currentSpecialProjectileIndex + 1) % this._projectilesSpecial.Count;
            this._lastTimeShoot = Time.time;
            this.ShootCount++;
        }

        public void ShootSpecialObject(GameObject shootObject)
        {
            GameObject shoot = (GameObject)GameObject.Instantiate(shootObject, this._specialShootPoint.transform.position, this.transform.rotation);
            Soldier shootSoldier = shoot.GetComponent<Soldier>();
            shootSoldier.IsSpecial = true;
        }

        protected void RotateTowardsMouse()
        {
            Vector2 mousePosition = this._camera.ScreenToWorldPoint(Input.mousePosition);
            var newUp = (mousePosition - (Vector2)this.transform.position).normalized;

            if (this._textAngle != null)
                this._textAngle.text = Vector3.Angle(newUp, Vector3.right).ToString();

            var angle = Vector3.Angle(newUp, Vector3.right);
            if (angle >= _minRotationAngle && angle <= _maxRotationAngle && Vector3.Dot(newUp, Vector3.up) > 0)
            {
                this.transform.rotation = Quaternion.LookRotation(Vector3.forward, newUp);
                this._minorCogObject.transform.rotation = Quaternion.Inverse(this.transform.rotation * this.transform.rotation);
            }
        }

        public void UpdateSoldierNameUI()
        {
            if (AmmunitionClip.Instance.IsOutOfAmmo)
            {
                LevelManager.Instance._soldierNameText.text = string.Empty;
                return;
            }

            if (GameManager.Instance.SoldierNames.ContainsKey(AmmunitionClip.Instance.NextAmmunition._id))
            {
                LevelManager.Instance._soldierNameText.text = string.Format("{0}", GameManager.Instance.SoldierNames[AmmunitionClip.Instance.NextAmmunition._id]);
            }
            else
			{
                LevelManager.Instance._soldierNameText.text = string.Format("{0}", "Vicenzito");
            }
        }

        public void RetrieveAll()
        {
            var soldiers =
            AmmunitionClip.Instance.ShootedSoldiers.ToList();
            for (int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].Die();
            }
        }

        public void NewAmmo()
        {
            if (this._waitingForAmmo)
            {
                this._waitingForAmmo = false;
                this.CallReloadAnimation();
            }
        }
    }
}