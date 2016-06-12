using UnityEngine;

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

            //this.transform.rotation = Quaternion.identity;
            //this.transform.position = Vector3.zero;

            //this.CurrentSortingOrder = 1;

            _maxSpeed /= 3;
            _minSpeed /= 3;
        }

        public override void Shoot()
        {
            this.RigidBody.AddForce(this.transform.up * this._maxSpeed * 0.8f, ForceMode2D.Impulse);
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.BASE)
            {
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == TagAndLayer.BASE)
            {
                collider.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
            }
        }

        public override void Recycle()
        {
            this.Start();
        }
    }
}