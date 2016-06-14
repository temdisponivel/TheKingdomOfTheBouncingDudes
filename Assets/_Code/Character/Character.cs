using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all characters.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        [Header("Attributes")]
        public float _pointsWhenKilled = 5f;
        public int _id = 0;
        public bool _affectedByElements = true;
        public bool _shouldRecycle = true;


        [Header("Stats")]
        protected int _damage = 1;
        public float _speed = 1f;
        public float _size = 1f; // Only for Game Design porpouses.
        public float _hp = 1f;
		public float _statSpeed = 1f; // Representation porpouses.

        protected float _maxSpeed = 1f;
        protected float _minSpeed = 1f;

        [Header("Effects")]
        protected bool _isShinyAttached = false;
        protected float _timeToTravel = .5f;

        private int _currentSortingOrder = 0;

        protected GameObject _originalGameObject;

        protected Rigidbody2D _rigid = null;
        protected Animator _animator = null;
        protected Collider2D _collider = null;
        protected SpriteRenderer _sprite = null;

        protected int _spriteOnBarrelOrder = 7, _spriteOnFieldOrder = 2;
        
        protected float _hpBkp = 1f;
        protected Quaternion _rotationBkp = Quaternion.identity;
        protected Vector3 _scaleBkp = Vector3.one;
        
        public GameObject OriginalGameObject { get { return this._originalGameObject; } set { this._originalGameObject = value; } }
        public Rigidbody2D RigidBody { get { return this._rigid; } }
        public Animator Animator { get { return this._animator; } }
        public Collider2D Collider { get { return this._collider; } }
        public SpriteRenderer Sprite { get { return this._sprite; } }
        public float HP { get { return this._hp; } set { this._hp = value; if (this._hp <= 0) { this.Die(); } } }
        public float Speed { get { return this._speed; } set { this._speed = value; } }
        public int Damage { get { return this._damage; } set { this._damage = value; } }
        public bool AffectedByElement { get { return this._affectedByElements; } set { this._affectedByElements = value; } }

        protected bool _onBarrel { get; set; }
        public bool _isSpecial = false;
        protected bool _transitioning { get; set; }

        public int CurrentSortingOrder
        {
            get
            {
                return this._currentSortingOrder;
            }
            set
            {
                this._currentSortingOrder = value;
                this.Sprite.sortingOrder = _currentSortingOrder;
            }
        
        }
        public float TimeToTravel { get { return this._timeToTravel; } }

        public int SpriteOnBarrelOrder { get { return _spriteOnBarrelOrder; } }
        public int SpriteOnFieldOrder { get { return _spriteOnFieldOrder; } }

        public bool IsShinyAttached { get { return this._isShinyAttached; } set { this._isShinyAttached = value; } }

        public virtual void Awake()
        {
            this._rigid = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider2D>();
            this._sprite = this.GetComponent<SpriteRenderer>();

            this._hpBkp = this._hp;
            this._rotationBkp = this.transform.rotation;
            this._scaleBkp = this.transform.localScale;

            this.ConvertSpeed();

            this._originalGameObject = this.gameObject;
        }

        public virtual void Start()
        {
            this.ConvertSpeed();
        }

        public virtual void Die()
        {
            if (this.OnDie != null)
                this.OnDie(this);

            if (this._shouldRecycle)
                this.Recycle();
            else
                GameObject.Destroy(this.gameObject);
        }

        public virtual void LateUpdate()
        {
            if (this._rigid.velocity.magnitude > this._maxSpeed)
            {
                this._rigid.velocity = this._rigid.velocity.normalized * this._maxSpeed;
            }
            else if (this._rigid.velocity.magnitude < this._minSpeed)
            {
                this._rigid.velocity = this._rigid.velocity.normalized * this._minSpeed;
            }
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
            _transitioning = true;
            this.StartCoroutine(this.WaitSecondsAndCall(.5f, () =>
            {
                this.CurrentSortingOrder = this._spriteOnFieldOrder;
                _transitioning = false;
            }));
            
            this._onBarrel = false;

            this._rigid.isKinematic = true;
            this._collider.enabled = false;
        }

        /// <summary>
        /// Turn the Ammunition character, which is out of the field, to a Projectile character, which is on the field.
        /// </summary>
        public void TurnIntoProjectile()
        {
            this.StartCoroutine(this.WaitSecondsAndCall(.5f, () =>
            {
                this.CurrentSortingOrder = this._spriteOnFieldOrder;
                _transitioning = false;
            }));

            this._onBarrel = false;

            this._rigid.isKinematic = false;
            this._collider.enabled = true;
        }

        public abstract void Recycle();
        public abstract void Shoot();

        public event Action<Character> OnDie;

        public IEnumerator WaitSecondsAndCall(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }
    }
}