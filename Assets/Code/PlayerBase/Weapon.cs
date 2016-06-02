using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        
        protected List<GameObject> _projectiles = new List<GameObject>();
        protected List<GameObject> _projectilesSpecial = new List<GameObject>();


		protected float _minShootMultiplier = 0.45f;
		protected float _maxShootMultiplier = 0.8f;
        public float _coolDown = 1f;


		[Header("Objects")]
		public GameObject _idlePoint = null;
		public GameObject _shootPoint = null;
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

        public int ShootCount { get; set; }
        public float ForceMultiplier { get { return this._currentForceMultiplier; } }
		public Quaternion WeaponRotation { get { return this.transform.rotation; } }

        public void Start()
        {
            Weapon.Instance = this;
            this._projectiles = GameManager.Instance.GetAvailableSoldiers();
            this._projectilesSpecial = GameManager.Instance._specialProjectiles;

			this._weaponAnimator = this.GetComponent<Animator> ();
        }

        public void Update()
        {
            this.ShootRoutine();

            if (!this._special && Input.GetButtonUp("Special") && (Time.time - (this._specialStartTime + this._specialDuration)) >= this._coolDownBetweenSpecials)
            {
                Debug.Log("SPECIAL");
                this._special = true;
                this._specialStartTime = Time.time;
            }
            else
            {
                this._special = this._special && (Time.time - this._specialStartTime) <= this._specialDuration;
            }
        }


        /// <summary>
        /// Perform the logic of shooting.
        /// </summary>
        protected void ShootRoutine()
        {
            if (Input.GetMouseButton(0))
            {
                if (!this._holding)
                {
                    //this._ray.enabled = true;
                    this._holding = true;
                    this._currentForceMultiplier = 0;
					this._weaponAnimator.SetTrigger ("Holding");
                }

                this.RotateTowardsMouse();
                if (this._currentForceMultiplier <= this._maxShootMultiplier)
                {
					if (this._currentForceMultiplier >= this._minShootMultiplier)
						this._currentForceMultiplier += Time.deltaTime;
					else
						this._currentForceMultiplier = this._minShootMultiplier;
                }
            }
            else if (this._holding)
            {
                if (this._special)
                {
                    this._currentForceMultiplier = this._maxShootMultiplier;
                    this.ShootSpecial();
                }
                else if (Time.time - this._lastTimeShoot >= this._coolDown)
                {
					this._weaponAnimator.SetTrigger ("Shooting"); // Calls Shoot() on the animation clip.
                }
                this._holding = false;
            }
        }

        /// <summary>
        /// Shoot the current projectile.
        /// </summary>
        public void Shoot()
        {
			AmmunitionClip.Instance.ShootNextAmmunition ();
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

		public void ShootSpecialObject(GameObject shootObject){
			GameObject shoot = (GameObject)GameObject.Instantiate(shootObject, this._shootPoint.transform.position, this.transform.rotation);
			Soldier shootSoldier = shoot.GetComponent<Soldier> ();
			shootSoldier.IsSpecial = true;
		}

        protected void RotateTowardsMouse()
        {
            Vector3 mousePosition = this._camera.ScreenToWorldPoint(Input.mousePosition);
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePosition - this.transform.position).normalized);
			this._minorCogObject.transform.rotation = Quaternion.Inverse (this.transform.rotation * this.transform.rotation);

			this._textAngle.text = "" + this.transform.rotation.eulerAngles + "º";
        }


		/*
        public void Move()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 clickPosition = this._camera.ScreenToWorldPoint(Input.mousePosition);
                foreach (var movePoint in this._movePoints)
                {
                    if (Vector2.Distance(clickPosition, movePoint.transform.position) <= .1)
                    {
                        this.transform.position = movePoint.transform.position;
                        this._lastTimeShoot = Time.time;
                        break;
                    }
                }
                
            }
        }
        */
    }
}