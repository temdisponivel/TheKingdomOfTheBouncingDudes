using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all characters.
    /// </summary>
    public class Character : MonoBehaviour
    {
        [Header("Attributes")]
        public int _id = 1;
        public Color _baseColor = Color.white;
        public int _damage = 1;
        public int _hp = 1;
        public float _velocity = 1f;

        protected Rigidbody2D _rigid = null;
        protected Animator _animator = null;
        protected Collider2D _collider = null;

        public Rigidbody2D RigidBody { get { return this._rigid; } }
        public Animator Animator { get { return this._animator; } }
        public Collider2D Collider { get { return this._collider; } }
        public int HP { get { return this._hp; } set { this._hp = value; } }
        public float Velocity { get { return this._velocity; } set { this._velocity = value; } }
        public int Damage { get { return this._damage; } set { this._damage = value; } }

        virtual public void Start()
        {
            this._rigid = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider2D>();
        }
    }
}