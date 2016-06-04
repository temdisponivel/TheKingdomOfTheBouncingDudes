using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {   

		protected int _ammunitionPosition = -1;
		protected int _spriteOrderOnAmmunition = 9, _spriteOrderOnBarrel = 16, _spriteOrderOnField = 2;
        protected bool _elementHit = false;
		protected bool _isSpecial = false;
		protected bool _turnedIntoProjectile = false;

		public int AmmunitionPosition { get { return this._ammunitionPosition; } set { this._ammunitionPosition = value; } }
		public int SpriteOrderOnField { get { return _spriteOrderOnField; } }
		public int SpriteOrderOnBarrel { get { return _spriteOrderOnBarrel; } }
		public int SpriteOrderOnAmmunition { get { return _spriteOrderOnAmmunition; } }
		public bool IsSpecial { get { return _isSpecial; } set { _isSpecial = value; } }


		public override void Start(){
			
			base.Start ();

			this.CurrentSortingOrder = this._spriteOrderOnAmmunition;

			if (this.IsSpecial || this.tag == TagAndLayer.SOLDIER_CELL_COPY) {
				this.Shoot ();
			}
		}

        public void Shoot()
        {
			this.transform.parent = null;
			this.transform.rotation = Weapon.Instance.WeaponRotation;
			this.TurnIntoTransition ();
			this.RigidBody.AddForce(this.transform.up * this._maxSpeed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);

        }
			       
		public override void Update(){

			base.Update ();

			// Passed the cannon shoot point, turn it into a projectile.
			if (this.transform.position.y >= AmmunitionClip.Instance._changeToProjectilePoint.transform.position.y && !_turnedIntoProjectile) {

				this.TurnIntoProjectile ();
				_turnedIntoProjectile = true;
			}
			if (this.transform.position.y >= AmmunitionClip.Instance._changeToFieldOrderPoint.transform.position.y){
				if (this.CurrentSortingOrder != this.SpriteOrderOnField)
					this.CurrentSortingOrder = this._spriteOrderOnField;
			}

		}
			

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {

			string collTag = collision.gameObject.tag;

			if (collTag == TagAndLayer.ENEMY_BASE)
            {
				EffectManager.Instance.CreateDieEffect (this.transform);
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
				this.HP -= 1;
            }
			else if (collTag == TagAndLayer.WALL || collTag == TagAndLayer.BASE)
			{
				EffectManager.Instance.CreateWallHitEffect (this.transform);
			}
            
            if (collision.gameObject.layer == TagAndLayer.GAME_OBJECTS)
            {
                this._elementHit = true;
            }
        }

        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
				Character monster = collider.gameObject.GetComponent<Character>();
                this.HP -= 1;
                monster.HP -= 1;

				EffectManager.Instance.CreateHitEffect (monster.transform);

                if (monster.HP <= 0)
                {
                    EffectManager.Instance.CreateDieEffect(monster.transform);
                    EffectManager.Instance.CreateSmokeEffect(monster.transform);
                    LevelManager.Instance.KillEnemy(monster);
                    ComboManager.Instance.AddKill();
                    ComboManager.Instance.AddElementKill();
                }
                else
                {
                    ComboManager.Instance.AddHit();
                }
            }

            if (collider.gameObject.layer == TagAndLayer.GAME_OBJECTS)
            {
                this._elementHit = true;
            }
        }

        override public void Die()
        {
			if (!this._isSpecial && this.tag != TagAndLayer.SOLDIER_CELL_COPY)
				AmmunitionClip.Instance.AddAmmunition (this.OriginalGameObject);
			
            GameObject.Destroy(this.gameObject);
        }

		public void OnBarrel(){
			this.CurrentSortingOrder = this._spriteOrderOnBarrel;
		}

    }
}