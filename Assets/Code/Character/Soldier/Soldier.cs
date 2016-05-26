using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {   
		protected int _sortOrderBefore = 13, _sortOrderAfter = 2;
        protected bool _elementHit = false;

        override public void Start()
        {
            base.Start();
            this.Shoot();
        }

        virtual public void Shoot()
        {
			this.RigidBody.AddForce(this.transform.up * this._maxSpeed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
			Debug.Log (this.RigidBody.velocity);
			_sprite.sortingOrder = this._sortOrderBefore;
        }
        
        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
			_sprite.sortingOrder = this._sortOrderAfter;
			string collTag = collision.gameObject.tag;

			if (collTag == TagAndLayer.ENEMY_BASE)
            {
				EffectManager.Instance.CreateDieEffect (this.transform);
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
				this.HP -= 1;
                //this.Die();
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
            GameObject.Destroy(this.gameObject);
        }
    }
}