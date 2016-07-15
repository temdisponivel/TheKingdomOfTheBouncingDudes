using UnityEngine;
using DG.Tweening;
using Assets._Code.Game;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all monsters.
    /// </summary>
    public class Monster : Character
    {
		public int _monsterType;
		protected int _soundMinValue, _soundMaxValue;

		protected Quaternion _shootRotation;
        protected Quaternion _fixedRotation;
		protected bool _shooted = false;

        public bool _dead = false;
        
        public override void Start()
        {
            base.Start();

			this._shootRotation = this.transform.rotation;
            this._fixedRotation = new Quaternion(0, 0, 0, 1.0f);
            //this.transform.position = Vector3.zero;


			if (GameManager.Instance.CurrentLevel.Id != LevelId.FIFTEEN)
				this.JumpOverTheFenceAnimation ();
			else
				this.Shoot ();
				
            _maxSpeed /= 3;
            _minSpeed /= 3;
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

			if (this.transform.rotation != this._fixedRotation && _shooted)
            {
                this.transform.rotation = this._fixedRotation;
            }

        }

		public void JumpOverTheFenceAnimation(){
			this.Collider.enabled = false;
			this.StartCoroutine (this.WaitSecondsAndCall (0.000001f, () => { // I used this because for some reason SpriteOrder goes to 0 after Start
				this.transform.rotation = this._fixedRotation;
				this.CurrentSortingOrder = -2;
			}));

			this.transform.DOMoveY (this.transform.position.y + 1.5f, 0.4f).SetEase(Ease.OutBack).OnComplete(() => {
				this.CurrentSortingOrder = 1;
				this.PlaySpawnSound ();
				this.transform.DOMoveY (this.transform.position.y - 3f, 0.4f).SetEase(Ease.OutSine).OnComplete(() => {
					this.Shoot();
				});
			});
		}

        public override void Shoot()
        {
			if (GameManager.Instance.CurrentLevel.Id == LevelId.FIFTEEN)
				this.PlaySpawnSound ();

			this.CurrentSortingOrder = 1;
			this.transform.rotation = _shootRotation;
			this.Collider.enabled = true;

            this.RigidBody.AddForce(this.transform.up * this._maxSpeed * 0.8f, ForceMode2D.Impulse);
			_shooted = true;
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.BASE)
            {
				this.PlayHitSound ();
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == TagAndLayer.BASE)
            {
				this.PlayHitSound ();
                collider.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
        }
			

        public override void Die()
        {
            if (_dead)
                return;

            _dead = true;

			if (ArcadeManager.Instance != null)
				ArcadeManager.Instance.AddArcadePoints (100);

			EffectManager.Instance.CreateDieEffect(this.transform);
			EffectManager.Instance.CreateSmokeEffect(this.transform);

            base.Die();
        }

        public override void Recycle()
        {
            this.Start();
        }

		public void PlaySpawnSound(){
			
			this.SetSpawnSoundValues ();
			int random = (int)Random.Range (_soundMinValue, _soundMaxValue);
			AudioManager.Instance.PlayMonsterSound (random);

		}

		public void PlayHitSound(){

			this.SetHitSoundValues ();
			int random = (int)Random.Range (_soundMinValue, _soundMaxValue);
			AudioManager.Instance.PlayMonsterSound (random);

		}

		// SOUNDS BASED ON THE AUDIO MANAGER > MONSTERS SFX ORDER
		protected void SetSpawnSoundValues(){
			switch (_monsterType) {
			case 0:
				_soundMinValue = 0;
				_soundMaxValue = 2;
				break;
			case 1:
				_soundMinValue = 6;
				_soundMaxValue = 6;
				break;
			case 2:
				_soundMinValue = 8;
				_soundMaxValue = 9;
				break;
			default:
				break;
			}
		}

		protected void SetHitSoundValues(){
			switch (_monsterType) {
			case 0:
				_soundMinValue = 3;
				_soundMaxValue = 5;
				break;
			case 1:
				_soundMinValue = 7;
				_soundMaxValue = 7;
				break;
			case 2:
				_soundMinValue = 10;
				_soundMaxValue = 11;
				break;
			default:
				break;
			}
		}
    }
}