using UnityEngine;
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

        public Camera _camera = null;

        [Header("Shooting settings")]
        public List<GameObject> _projectiles = new List<GameObject>();

        [Tooltip("Multiplier (in seconds) for the weapon.")]
        public float _forceShootMultiplier = 1f;
        public float _coolDown = 1f;
        public float _limitForceMultiplier = 10f;

        [Header("Visual settings")]
        public SpriteRenderer _ray = null;

        protected float _lastTimeShoot = 0f;
        protected float _currentForceMultiplier = 0f;
        protected bool _holding = false;
        protected int _currentProjectileIndex = 0;

        public float ForceMultiplier { get { return this._currentForceMultiplier; } }

        public void Start()
        {
            if (Weapon.Instance == null)
            {
                Weapon.Instance = this;
            }
        }

        public void Update()
        {
            this.ShootRoutine();
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
                    this._ray.enabled = true;
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
                this.Shoot();
                this._ray.enabled = false;
                this._holding = false;
            }
        }

        /// <summary>
        /// Shoot the current projectile.
        /// </summary>
        public void Shoot()
        {
            GameObject projectile = (GameObject)GameObject.Instantiate(this._projectiles[this._currentProjectileIndex], this.transform.position, this.transform.rotation);
            this._currentProjectileIndex = (this._currentProjectileIndex + 1) % this._projectiles.Count;
        }

        protected void RotateTowardsMouse()
        {
            Vector3 mousePosition = this._camera.ScreenToWorldPoint(Input.mousePosition);
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePosition - this.transform.position).normalized);
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
    }
}