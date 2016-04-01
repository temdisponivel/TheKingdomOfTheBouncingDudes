using UnityEngine;
using System.Collections;

namespace BounceDudes.Base
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : MonoBehaviour
    {
        [Header("Attributes")]
        public Color _baseColor = Color.white;
        public float _hp = 1f;
        public float _resistence = 1f;
        public float _velocity = 1f;
        public float _size = 1f;

        [Header("Control")]
        public bool _affectedByTower = true;
        public bool _killedWhenStop = true;

        protected Rigidbody2D _rigid = null;
        protected Animator _animator = null;

        public Rigidbody2D RigidBody { get { return this._rigid; } }
        public Animator Animator { get { return this._animator; } }
        public float HP { get { return this._hp; } set { this._hp = value; } }
        public float Resistence { get { return this._resistence; } set { this._resistence = value; } }
        public float Velocity { get { return this._velocity; } set { this._velocity = value; } }
        public float Size { get { return this._size; } set { this._size = value; } }
        
        virtual public void Start()
        {
            this._rigid = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this.Shoot();
        }

        virtual public void Shoot()
        {
            this.RigidBody.AddForce(Weapon.Instance.transform.up * this._velocity * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
        }

        virtual public void FixedUpdate()
        {
            if (this._killedWhenStop && this.RigidBody.velocity.magnitude <= .01)
            {
                this.Die();
            }
        }

        virtual public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}