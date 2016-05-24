using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a hitter. A object that launch (or hit if you will) another object.
    /// </summary>
    public class Hitter : MonoBehaviour
    {
        public float _hitForceMultiplier = 1.1f;

		protected Animator _animator = null;
		protected CircleCollider2D _collider = null;

		public void Start()
		{
			this._animator = this.GetComponent<Animator> ();
			this._collider = this.GetComponent<CircleCollider2D> ();
		}

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            Rigidbody2D rigid = null;
            if ((rigid = collision.gameObject.GetComponent<Rigidbody2D>()) != null)
            {
				ContactPoint2D contact = collision.contacts[0];
				if (contact.point.x <= _collider.bounds.center.x){
					this._animator.SetTrigger("LeftContact");
				}
				else{
					this._animator.SetTrigger("RightContact");
				}

				EffectManager.Instance.CreateWallHitEffect (collision.collider.transform);

                rigid.AddForce(rigid.velocity * this._hitForceMultiplier, ForceMode2D.Impulse);

            }
        }
    }
}