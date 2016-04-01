using UnityEngine;
using System.Collections;

namespace BounceDudes.Base
{
    /// <summary>
    /// Element portal. Used to teleport on object from one position to another.
    /// </summary>
    public class Portal : MonoBehaviour
    {
        public Portal _otherEnd = null;
        public GameObject LastTransported { get; set; }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._otherEnd.LastTransported != collider.gameObject && collider.gameObject.layer != TagAndLayer.NOT_TRANSPORTABLE)
            {
                this.LastTransported = collider.gameObject;
                collider.gameObject.transform.position = this._otherEnd.transform.position;
            }
        }
    }
}