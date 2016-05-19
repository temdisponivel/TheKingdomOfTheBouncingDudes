using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {        
		
        override public void Start()
        {
            base.Start();
            this.Shoot();
        }

        virtual public void Shoot()
        {
            this.RigidBody.AddForce(this.transform.up * this._speed * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
			_sprite.sortingOrder = 3;
        }
        
        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.ENEMY_BASE)
            {
                collision.gameObject.GetComponent<Base>().HP += this.Damage;
				this.HP -= 1;
                //this.Die();
            }
        }

        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                Character character = collider.gameObject.GetComponent<Character>();
                this.HP -= 1;
                character.HP -= 1;

                if (character.HP <= 0)
                {
                    LevelManager.Instance.KillEnemy(character);
                }
            }
        }

        override public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}