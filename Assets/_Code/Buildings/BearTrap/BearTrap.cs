using UnityEngine;
using System.Collections;

namespace BounceDudes
{
	/// <summary>
	/// Class that holds one object for a short time, then releases back into action.
	/// </summary>
	public class BearTrap : MonoBehaviour {

		[Tooltip("Time hold in the trap")]
		public float _holdTime = 1f;

		[Tooltip("Scalar to multiply the velocity of reshoot. 1 for the same velocity as initial.")]
		public float _forceMultiplier = 1f;

		protected GameObject _object = null;
		protected Animator _animator = null;

		// Use this for initialization
		void Start () {
			_animator = GetComponent<Animator> ();
		}

		public void OnTriggerEnter2D(Collider2D collider)
		{
			if (this._object != null || (collider.gameObject.layer != TagAndLayer.ENEMY_OBJECTS && collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)) {
				return;
			}

			EffectManager.Instance.CreateDieEffect(this.transform);

			this._animator.SetTrigger ("Close");
			this._animator.ResetTrigger ("Open");
			this._object = collider.gameObject;
			this._object.GetComponent<Rigidbody2D>().isKinematic = true;
			this._object.transform.position = this.transform.position;
			this.StartCoroutine(this.Shoot(this._object));

		}

		public void OnTriggerExit2D(Collider2D collider)
		{
			if (this._object == collider.gameObject)
			{
				this._object = null;
			}
		}

		public IEnumerator Shoot(GameObject obj)
		{
			yield return new WaitForSeconds(this._holdTime);
			if (this._object != null) {
				Rigidbody2D body = this._object.GetComponent<Rigidbody2D> ();
				body.isKinematic = false;

				this._animator.SetTrigger ("Open");
				this._animator.ResetTrigger ("Close");
				body.AddForce (-this._object.transform.up * this._object.GetComponent<Character> ().Speed * this._forceMultiplier, ForceMode2D.Impulse);

			} else {
				this._animator.SetTrigger ("Open");
				this._animator.ResetTrigger ("Close");
			}
		}
	}
}
