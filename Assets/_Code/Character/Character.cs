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
        public bool _affectedByElements = true;


        [Header("Stats")]
        protected int _damage = 1;
        public float _speed = 1f;
        public float _size = 1f; // Only for Game Design porpouses.
        public float _hp = 1f;

        protected float _maxSpeed = 1f;
        protected float _minSpeed = 1f;

        [Header("Effects")]
        protected bool _isShinyAttached = false;

        protected int _currentSortingOrder = 2;
        protected bool _movingAnimation = false;
        protected Vector3 _positionToMove;
        protected float _animationSpeed = 1.0f;

        protected GameObject _originalGameObject;

        protected Rigidbody2D _rigid = null;
        protected Animator _animator = null;
        protected Collider2D _collider = null;
        protected SpriteRenderer _sprite = null;

        public GameObject OriginalGameObject { get { return this._originalGameObject; } set { this._originalGameObject = value; } }
        public Rigidbody2D RigidBody { get { return this._rigid; } }
        public Animator Animator { get { return this._animator; } }
        public Collider2D Collider { get { return this._collider; } }
        public SpriteRenderer Sprite { get { return this._sprite; } }
        public float HP { get { return this._hp; } set { this._hp = value; if (this._hp <= 0) { this.Die(); } } }
        public float Speed { get { return this._speed; } set { this._speed = value; } }
        public int Damage { get { return this._damage; } set { this._damage = value; } }
        public bool AffectedByElement { get { return this._affectedByElements; } set { this._affectedByElements = value; } }
        public int CurrentSortingOrder { get { return _currentSortingOrder; } set { _currentSortingOrder = value; } }

        public bool IsShinyAttached { get { return this._isShinyAttached; } set { this._isShinyAttached = value; } }

        virtual public void Awake()
        {
            this._rigid = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider2D>();
            this._sprite = this.GetComponent<SpriteRenderer>();

            this._originalGameObject = this.gameObject;
        }

        virtual public void Start()
        {
            this.ConvertSpeed();
            this.TurnIntoAmmunition();
        }

        virtual public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }

        virtual public void Update()
        {
            this.Sprite.sortingOrder = _currentSortingOrder;
        }

        virtual public void LateUpdate()
        {

            if (this._rigid.velocity.magnitude > this._maxSpeed)
            {
                this._rigid.velocity = this._rigid.velocity.normalized * this._maxSpeed;
            }
            else if (this._rigid.velocity.magnitude < this._minSpeed)
            {
                this._rigid.velocity = this._rigid.velocity.normalized * this._minSpeed;
            }

            //this._sprite.sortingOrder = 16;
        }

        /// <summary>
        /// Converts the Stat Speed in Physics Speed.
        /// </summary>
        protected void ConvertSpeed()
        {
            _maxSpeed = _speed * 2f;
            _minSpeed = _speed / 2f;
        }


        /// <summary>
        /// The initial state of every character. Also called when animating "dead" projectiles or taking them out from the field. 
        /// </summary>
        public void TurnIntoAmmunition()
        {
            this._rigid.isKinematic = true;
            this._collider.enabled = false;
        }

        /// <summary>
        /// Turn the Ammunition character, which is out of the field, to a Projectile character, which is on the field.
        /// </summary>
        public void TurnIntoProjectile()
        {
            this._rigid.isKinematic = false;
            this._collider.enabled = true;
        }

        /// <summary>
        /// Turn the Ammunition character, which is out of the field, to a Transitioning character, which is in between.
        /// </summary>
        public void TurnIntoTransition()
        {
            this._rigid.isKinematic = false;
            this._collider.enabled = false;
        }

        public void Recycle()
        {
            this.ConvertSpeed();
            this.TurnIntoAmmunition();

        }
    }
}