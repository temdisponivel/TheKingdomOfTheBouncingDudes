using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {   
		protected int _sortOrderBefore = 13, _sortOrderAfter = 1;
        protected bool _elementHit = false;

        override public void Start()
        {
            base.Start();
            this.Shoot();
        }

        virtual public void Shoot()
        {
            this.RigidBody.AddForce(this.transform.up * this._speed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
			_sprite.sortingOrder = this._sortOrderBefore;
        }
        
        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
			_sprite.sortingOrder = this._sortOrderAfter;

            if (collision.gameObject.tag == TagAndLayer.ENEMY_BASE)
            {
				EffectManager.Instance.CreateDieEffect (this.transform);
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
				this.HP -= 1;
                //this.Die();
            }
			else if (collision.gameObject.tag == TagAndLayer.WALL)
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
                Character character = collider.gameObject.GetComponent<Character>();
                this.HP -= 1;
                character.HP -= 1;

				EffectManager.Instance.CreateHitEffect (character.transform);

                if (character.HP <= 0)
                {
                    EffectManager.Instance.CreateDieEffect(character.transform);
                    EffectManager.Instance.CreateSmokeEffect(character.transform);
                    LevelManager.Instance.KillEnemy(character);
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