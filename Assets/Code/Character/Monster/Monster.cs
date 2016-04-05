using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all monsters.
    /// </summary>
    public class Monster : Character
    {
        public override void Start()
        {
            base.Start();
            this.RigidBody.AddForce(this.transform.up * this._velocity, ForceMode2D.Impulse);
        }

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.BASE)
            {
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
            else if (collision.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
                this.HP -= collision.gameObject.GetComponent<Character>().Damage;
            }
        }

        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == TagAndLayer.BASE)
            {
                collider.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
        }
    }
}