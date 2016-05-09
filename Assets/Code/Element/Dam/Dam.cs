using UnityEngine;
using System.Collections;

namespace BounceDudes
{

    /// <summary>
    /// Class that slow down anything that passes through.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Dam : MonoBehaviour
    {
        [Tooltip("Number by which a character's velocity will be subtracted per second.")]
        public float _coeficient = 1.0f;

        [Tooltip("Force mode used to subtracted the character's velocity. Inpulse is a much faster reaction.")]
        public ForceMode2D _forceMode = ForceMode2D.Impulse;

        public void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS || collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(-collider.gameObject.transform.up * this._coeficient * Time.deltaTime, this._forceMode);
            }
        }
    }
}