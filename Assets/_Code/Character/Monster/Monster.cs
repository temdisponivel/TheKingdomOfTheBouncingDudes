using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all monsters.
    /// </summary>
    public class Monster : Character
    {
		protected Quaternion _fixedRotation;

        public override void Start()
        {
			base.Start();

			this.TurnIntoProjectile ();

			this.RigidBody.AddForce(this.transform.up * this._maxSpeed * 0.8f, ForceMode2D.Impulse);
			this._fixedRotation = Quaternion.identity;
			this.transform.rotation = _fixedRotation;

			this._currentSortingOrder = 1;

			_maxSpeed /= 3;
			_minSpeed /= 3;
        }

		public override void LateUpdate(){

			base.LateUpdate ();

			if (this.transform.rotation != _fixedRotation) {
				this.transform.rotation = _fixedRotation;
			}
		}

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == TagAndLayer.BASE)
            {
                collision.gameObject.GetComponent<Base>().HP -= this.Damage;
                this.Die();
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