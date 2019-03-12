using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class that holds one object for a short time, then releases back into action.
    /// </summary>
    public class BearTrap : MonoBehaviour
    {

        [Tooltip("Time hold in the trap")]
        public float _holdTime = 1f;

        [Tooltip("Scalar to multiply the velocity of reshoot. 1 for the same velocity as initial.")]
        public float _forceMultiplier = 1f;

        protected GameObject _object = null;
        protected Rigidbody2D _rigidBody2d = null;
        protected Animator _animator = null;

        protected int _objOrder = 0;

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._object != null || (collider.gameObject.layer != TagAndLayer.ENEMY_OBJECTS && collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS))
            {
                return;
            }

            if (!collider.gameObject.GetComponent<Character>().AffectedByElement)
                return;

            EffectManager.Instance.CreateDieEffect(this.transform);

            //_objOrder = collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            //collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder =
            //    this.GetComponent<SpriteRenderer>().sortingOrder - 1;

            this._animator.SetTrigger("Close");
            this._animator.ResetTrigger("Open");
            this._object = collider.gameObject;
            
            this._rigidBody2d = this._object.GetComponent<Rigidbody2D>();
            this._rigidBody2d.isKinematic = true;
            this._rigidBody2d.angularVelocity = 0f;
            this._rigidBody2d.velocity = Vector2.zero;
            
            this._object.transform.position = this.transform.position;
            this.StartCoroutine(this.Shoot(this._object));

        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            if (this._object == collider.gameObject)
            {
                //collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _objOrder;
                this._object = null;
            }
        }

        public IEnumerator Shoot(GameObject obj)
        {
            yield return new WaitForSeconds(this._holdTime);
            if (this._object != null)
            {
                
                _rigidBody2d.isKinematic = false;

                this._animator.SetTrigger("Open");
                this._animator.ResetTrigger("Close");
                _rigidBody2d.AddForce(this._object.transform.up * this._object.GetComponent<Character>().Speed * -this._forceMultiplier, ForceMode2D.Impulse);

            }
            else
            {
                this._animator.SetTrigger("Open");
                this._animator.ResetTrigger("Close");
            }
        }
    }
}
