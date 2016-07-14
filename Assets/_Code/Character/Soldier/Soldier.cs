using System;
using System.Linq;
using UnityEngine;

namespace BounceDudes
{


    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {
		public SoldierClassEnum _soldierClass;
        public string _soldierName = "Vicenzito";

		[TextArea(3, 10)]
		public string _soldierDescription = "Ele é um troxa";

        protected int _ammunitionPosition = -1;
        public int AmmunitionPosition { get { return this._ammunitionPosition; } set { this._ammunitionPosition = value; } }

		protected float _maxHP = 0;

        private bool _shooted = false;

        public override void Start()
        {
            base.Start();

			if (this._maxHP == 0)
				_maxHP = this.HP;
			
			this.Sprite.color = Color.white;

            if (!_shooted && !_isSpecial)
                this.TurnIntoAmmunition();
        }

        public override void Shoot()
        {
            if (!this._isSpecial)
                this.transform.rotation = Weapon.Instance.WeaponRotation;

            _transitioning = false;

            this.ConvertSpeed();

            this._shooted = true;

            this.TurnIntoProjectile();

            this.RigidBody.AddForce(this.transform.up * this._maxSpeed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            string collTag = collision.gameObject.tag;

			if (collTag == TagAndLayer.BASE) {
				AudioManager.Instance.PlayInterfaceSound (5);
				return;
			}

            if (collTag == TagAndLayer.ENEMY_BASE)
            {
				AudioManager.Instance.PlayInterfaceSound (6);
                EffectManager.Instance.CreateDieEffect(this.transform);
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
				this.SoldierReceiveHit ();
            }
            else if (collTag == TagAndLayer.WALL || collTag == TagAndLayer.BASE)
            {
				AudioManager.Instance.PlayInterfaceSound (5);
                EffectManager.Instance.CreateWallHitEffect(this.transform);
            }
            else if (collTag == TagAndLayer.BOSS)
            {
                EffectManager.Instance.CreateDieEffect(this.transform);
                collision.gameObject.GetComponent<BossBehaviour>().BossHP -= this.Damage;
				this.SoldierReceiveHit ();

            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                if (this._onBarrel || _transitioning)
                    return;

                Monster monster = collider.gameObject.GetComponent<Monster>();
				monster.HP -= 1;
				monster.PlayHitSound ();

				ComboManager.Instance.AddHit ();

				var monsterKilled = false;
                if (monster.HP <= 0)
                {
					// Elf Skill
					if (this._soldierClass == SoldierClassEnum.PRECISION) {
						EffectManager.Instance.CreatePrecisionHitEffect (monster.transform);
						this.HP += 1;
					} 
					else {
						EffectManager.Instance.CreateHitEffect (monster.transform);
					}
						
					monsterKilled = true;

					this.AddSpecialPoints (2f);

                    LevelManager.Instance.KillEnemy(this);
                    ComboManager.Instance.AddKill();
                    ComboManager.Instance.AddElementKill();
                }

				if (!monsterKilled)
					EffectManager.Instance.CreateHitEffect (monster.transform);
				

				this.AddSpecialPoints(2f);

				this.SoldierReceiveHit ();

            }
        }

		protected void SoldierReceiveHit(){
			this.HP -= 1;

			// Dwarf Skill
			if (this._soldierClass == SoldierClassEnum.BERSERK && this.HP <= this._maxHP / 2f) {
				this.IncrementMaxMinSpeed (0.5f);
				this._rigid.AddForce(this._rigid.velocity.normalized * 100f, ForceMode2D.Force);

				Color newColor = new Color (255f/255f, 128f/255f, 128f/255f, 1f);
				this.ChangeSpriteColor (newColor);
			}
		}

		public void ChangeSpriteColor(Color color){
			this.Sprite.color = color;
		}

		protected void AddSpecialPoints(float value){

			float auxValue = value;

			// Human Skill
			if (this._soldierClass == SoldierClassEnum.RESEARCH) {
				auxValue *= 2f;
			}
				
			Weapon.Instance.SpecialCurrentPoints += auxValue;
		}

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!_onBarrel)
                return;

            this.transform.position = Weapon.Instance._idlePoint.transform.position;
            this.transform.rotation = Weapon.Instance.WeaponRotation;
        }

        public void OnBarrel()
        {
            this.CurrentSortingOrder = this._spriteOnBarrelOrder;
            this._onBarrel = true;
        }

        public override void Recycle()
        {
            this._shooted = false;
            this._transitioning = true;
            this.Start();
            this._hp = _hpBkp;
            this.transform.rotation = _rotationBkp;
            this.transform.localScale = _scaleBkp;
            AmmunitionClip.Instance.AddAmmunition(this.gameObject);
        }
    }
}