using UnityEngine;
using System.Collections;

namespace BounceDudes
{

    /// <summary>
    /// Class that slow down anything that passes through.
    /// </summary>
	[RequireComponent(typeof(CircleCollider2D))]
    public class SlowTower : MonoBehaviour
    {
        [Tooltip("Number by which a character's velocity will be subtracted per second.")]
        public float _coeficient = -2f;

        [Tooltip("Force mode used to subtracted the character's velocity. Impulse is a much faster reaction.")]
        public ForceMode2D _forceMode = ForceMode2D.Impulse;

        public void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS || collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
				Rigidbody2D rigidBody = collider.gameObject.GetComponent<Rigidbody2D> ();
				rigidBody.AddForce(rigidBody.velocity.normalized * this._coeficient * Time.deltaTime, this._forceMode);
            }
        }
    }
}