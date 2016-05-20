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

        [Tooltip("Multiplier (in seconds) for the weapon.")]
        public float _forceShootMultiplier = 1f;
        public float _coolDown = 1f;
        public float _limitForceMultiplier = 10f;

		[Header("Objects")]
		public GameObject _shootPoint = null;
		public GameObject _cogObject = null;

        [Header("Visual settings")]
        //public SpriteRenderer _ray = null;
        public int _quantityOfShownMunition = 10;

        [Header("MovePoints")]
        public List<GameObject> _movePoints = null;


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

        public void Start()
        {
            Weapon.Instance = this;
            this._projectiles = GameManager.Instance.GetAvailableSoldiers();
            this._projectilesSpecial = GameManager.Instance._specialProjectiles;

			this._weaponAnimator = this.GetComponentInParent<Animator> ();
        }

        public void Update()
        {
            //this.Move();
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
                }
                this.RotateTowardsMouse();
                if (this._currentForceMultiplier <= this._limitForceMultiplier)
                {
                    this._currentForceMultiplier += this._forceShootMultiplier * Time.deltaTime;
                }
            }
            else if (this._holding)
            {
                if (this._special)
                {
                    this._currentForceMultiplier = this._limitForceMultiplier;
                    this.ShootSpecial();
                }
                else if (Time.time - this._lastTimeShoot >= this._coolDown)
                {
                    this.Shoot();
                }
                //this._ray.enabled = false;
                this._holding = false;
            }
        }

        /// <summary>
        /// Shoot the current projectile.
        /// </summary>
        public void Shoot()
        {
			this._weaponAnimator.SetTrigger ("Shooting");

            this.ShootObject(this._projectiles[this._currentProjectileIndex]);
            this._currentProjectileIndex = (this._currentProjectileIndex + 1) % this._projectiles.Count;
            this._lastTimeShoot = Time.time;
            this.ShootCount++;
        }

        /// <summary>
        /// Shoot the current projectile.
        /// </summary>
        public void ShootSpecial()
        {
            this.ShootObject(this._projectilesSpecial[this._currentSpecialProjectileIndex]);
            this._currentSpecialProjectileIndex = (this._currentSpecialProjectileIndex + 1) % this._projectilesSpecial.Count;
            this._lastTimeShoot = Time.time;
            this.ShootCount++;
        }

        public void ShootObject(GameObject shoot)
        {
			GameObject.Instantiate(shoot, this._shootPoint.transform.position, this.transform.rotation);
        }

        protected void RotateTowardsMouse()
        {
            Vector3 mousePosition = this._camera.ScreenToWorldPoint(Input.mousePosition);
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePosition - this.transform.position).normalized);
			_cogObject.transform.rotation = Quaternion.Inverse (this.transform.rotation * this.transform.rotation);

			_textAngle.text = "" + this.transform.rotation.eulerAngles + "º";
        }

        public void OnDrawGizmos()
        {
            Vector3 mousePosition = Camera.current.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(this.transform.position, (mousePosition - this.transform.position));
            }
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