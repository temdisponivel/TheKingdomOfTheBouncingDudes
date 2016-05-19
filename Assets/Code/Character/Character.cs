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
        public float _pointsWhenKilled = 5f;
        public int _id = 1;
        public bool _affectedByElement = true;
        public Color _baseColor = Color.white;
        public int _damage = 1;
		public float _speed = 1f;
		public float _size = 1f; // Only for Game Design porpouses.
        public float _hp = 1;

        protected Rigidbody2D _rigid = null;
        protected Animator _animator = null;
        protected Collider2D _collider = null;
		protected SpriteRenderer _sprite = null;

        public Rigidbody2D RigidBody { get { return this._rigid; } }
        public Animator Animator { get { return this._animator; } }
        public Collider2D Collider { get { return this._collider; } }
		public SpriteRenderer Sprite { get { return this._sprite; } }
        public float HP { get { return this._hp; } set { this._hp = value; if (this._hp <= 0) { this.Die(); } } }
        public float Speed { get { return this._speed; } set { this._speed = value; } }
        public int Damage { get { return this._damage; } set { this._damage = value; } }
        public bool AffectedByElement { get { return this._affectedByElement; } set { this._affectedByElement = value; } }

        virtual public void Start()
        {
            this._rigid = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider2D>();
			this._sprite = this.GetComponent<SpriteRenderer>();
        }

        virtual public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
			
    }
}