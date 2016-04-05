using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all soldiers.
    /// </summary>
    public class Soldier : Character
    {        
        virtual public void Start()
        {
            base.Start();
            this.Shoot();
        }

        virtual public void Shoot()
        {
            this.RigidBody.AddForce(Weapon.Instance.transform.up * this._velocity * Weapon.Instance.ForceMultiplier, ForceMode2D.Impulse);
        }

        virtual public void FixedUpdate()
        {
            if (this.RigidBody.velocity.magnitude <= .01)
            {
                this.Die();
            }
        }

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.ENEMY_BASE)
            {
                collision.gameObject.GetComponent<Base>().HP += this.Damage;
                this.Die();
            }
        }

        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                Character character = collider.gameObject.GetComponent<Character>();
                this.HP -= character.Damage;
                character.HP -= this.Damage;
            }
        }

        virtual public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}