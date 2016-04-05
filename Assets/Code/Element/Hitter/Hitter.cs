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

        virtual public void OnCollisionEnter2D(Collision2D collision)
        {
            Rigidbody2D rigid = null;
            if ((rigid = collision.gameObject.GetComponent<Rigidbody2D>()) != null)
            {
                rigid.AddForce(rigid.velocity * this._hitForceMultiplier, ForceMode2D.Impulse);
            }
        }
    }
}