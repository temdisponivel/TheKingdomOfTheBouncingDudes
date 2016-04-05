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
                Base.Instance.HP -= this.Damage;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}