using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class similar to hitter, but that aims in something.
    /// </summary>
    public class SmartHitter : MonoBehaviour
    {
        [Tooltip("Delay to shoot object")]
        public float _delay = 1f;

        [Tooltip("Distance to detected near object to shoot. This should be a radius for the circle cast.")]
        public float _radius = 10f;

        [Tooltip("Scalar to multiplie the velocity of reshoot. 1 for the same velocity as initial.")]
        public float _forceMultiplier = 1f;

        protected GameObject _object = null;
        protected int layer = 0;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._object != null || (collider.gameObject.layer != TagAndLayer.ENEMY_OBJECTS && collider.gameObject.layer != TagAndLayer.PLAYER_OBJECTS))
            {
                return;
            }
            if (collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
                this.layer = TagAndLayer.ENEMY_OBJECTS;
            }
            else
            {
                this.layer = TagAndLayer.PLAYER_OBJECTS;
            }
            this._object = collider.gameObject;
            this._object.GetComponent<Rigidbody2D>().isKinematic = true;
            this._object.transform.position = this.transform.position;
            this.StartCoroutine(this.Shoot(this._object));
        }

        public IEnumerator Shoot(GameObject obj)
        {
            yield return new WaitForSeconds(this._delay);
            if (this._object != null)
            {
                Rigidbody2D body = this._object.GetComponent<Rigidbody2D>();
                body.isKinematic = false;

                Collider2D hit = Physics2D.OverlapCircle(this.transform.position, this._radius, 1 << this.layer);
                if (hit != null)
                {
                    Debug.Log(hit.gameObject);
                    body.AddForce((hit.gameObject.transform.position - this.transform.position).normalized * this._object.GetComponent<Character>().Speed * this._forceMultiplier, ForceMode2D.Impulse);
                }
                else
                {
                    body.AddForce(this._object.transform.up * this._object.GetComponent<Character>().Speed * this._forceMultiplier, ForceMode2D.Impulse);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            if (this._object == collider.gameObject)
            {
                this._object = null;
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.transform.position, this._radius);
        }
    }
}