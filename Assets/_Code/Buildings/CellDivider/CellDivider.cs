using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a trench that shoots objects from time to time.
    /// </summary>
    public class CellDivider : MonoBehaviour
    {
        [Header("Copies Slime Ball")]
        public GameObject _slimeObject = null;
        public float _slimeScaleMultiplier = 1.3f;

        [Header("Cell Divider Settings")]
        [Tooltip("Point from which the player objects will be shooted.")]
        public GameObject _shootPointPlayer = null;

        [Tooltip("Point from which the enemyobjects will be shooted.")]
        public GameObject _shootPointEnemy = null;

        [Tooltip("Targets to shoot when enemy is in.")]
        public GameObject[] _targetsEnemy = null;

        [Tooltip("Targets to shoot when player is in.")]
        public GameObject[] _targetsPlayer = null;

        [Tooltip("Cooldown between shoots.")]
        public float _coolDown = 0f;

        [Tooltip("Quantity to shoot.")]
        public int _maxShoots = 3;

        protected Rigidbody2D _rigidBody2d = null;
        
        protected Animator _animator = null;
        protected GameObject _shootPoint = null;
        protected GameObject _toShoot = null;
        protected Character _inside = null;
        protected GameObject[] _targets = null;
        protected float _lastTimeShoot = 0;
        protected int _currentShootCount = 0;

        protected Vector3 _insideScaleBkp = Vector3.zero;

        protected bool _free = true;

		protected bool _isMonster = false;

        Vector3 diff;

        public void Start()
        {
            this._animator = this.GetComponent<Animator>();
        }

        public void Update()
        {
            if (this._currentShootCount == this._maxShoots || this._inside == null)
            {
                if (this._inside != null)
                {
                    this._inside.GetComponent<Character>().Die();
                }
                this.Clear();   
            }

            if (!this._free && Time.time - this._lastTimeShoot >= this._coolDown)
            {
                this.CreateCopy(); // Shoot
            }
        }

        public void Clear()
        {
            this._free = true;
            //GameObject.Destroy (this._inside);
            this._toShoot = null;
            this._inside = null;
            this._animator.SetBool("Shooting", false); // Para animação de atirando
        }

        protected void CreateCopy()
        {
			GameObject target = this._targets[this._currentShootCount];

            if (_toShoot == null)
                this.Clear();

            GameObject character = (GameObject)GameObject.Instantiate(this._toShoot, this._shootPoint.transform.position,
                Quaternion.LookRotation(Vector3.forward, diff = (target.transform.position - this._shootPoint.transform.position).normalized));

            character.tag = TagAndLayer.SOLDIER_CELL_COPY;

            GameObject cellSlime = EffectManager.Instance.AttachSlimeEffect(character.transform);

            character.transform.localScale = character.transform.localScale / this._maxShoots;
            cellSlime.transform.localScale = character.transform.localScale * _slimeScaleMultiplier;

            this._inside.transform.localScale = this._insideScaleBkp - ((this._insideScaleBkp / this._maxShoots) * (this._currentShootCount + 1));


            Character charScript = character.GetComponent<Character>();
            charScript.HP = 1;// Clones can only have 1 life.
            this._lastTimeShoot = Time.time;
            this._currentShootCount++;
            charScript._shouldRecycle = false;
            charScript._isSpecial = true;

			if (!_isMonster)
           		charScript.Shoot();
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            // Return if the Cell Divider is not free, or the entring collider is already a copy, or if the object is not a Player or an Enemy projectile.
            if (!this._free || collider.gameObject.tag == TagAndLayer.SOLDIER_CELL_COPY || (collider.gameObject.layer != TagAndLayer.PLAYER_OBJECTS && collider.gameObject.layer != TagAndLayer.ENEMY_OBJECTS))
            {
                return;
            }

            Character character = collider.gameObject.GetComponent<Character>();
            if (character._isSpecial)
                return;

            character.OnDie += OnDie;

            this._animator.SetBool("Shooting", true); // Inicia animação de atirando

            this._rigidBody2d = collider.gameObject.GetComponent<Rigidbody2D>();
            this._rigidBody2d.isKinematic = true;
            this._rigidBody2d.angularVelocity = 0f;
            this._rigidBody2d.velocity = Vector2.zero;

            collider.gameObject.transform.position = this._slimeObject.transform.position;
            
            this.Add(character);
        }

        public void Add(Character gameObject)
        {
            this._free = false;
            this._currentShootCount = 0;
            this._lastTimeShoot = Time.time;
            this._insideScaleBkp = gameObject.transform.localScale;
            this._inside = gameObject;
            if (gameObject.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
				this._isMonster = false;
                this._targets = this._targetsPlayer;
                this._shootPoint = this._shootPointPlayer;
                this._toShoot = gameObject.OriginalGameObject;
                //this._toShoot = GameManager.Instance.Soldiers[gameObject.GetComponent<Character>()._id];
            }
            else
            {
				this._isMonster = true;
                this._targets = this._targetsEnemy;
                this._shootPoint = this._shootPointEnemy;
                this._toShoot = GameManager.Instance.Monsters[gameObject.GetComponent<Character>()._id];
            }
        }


        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(this.transform.position, diff);
        }

        public void OnDie(Character character)
        {
            if (_inside == character)
            {
                this.Clear();
            }
        }
    }
}
