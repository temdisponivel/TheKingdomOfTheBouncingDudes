using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour {

	protected GameObject _objectToFollow;
	public GameObject ObjectToFollow { set { this._objectToFollow = value; } }


	void Update(){

		if (_objectToFollow != null) {
			// Ensures that the shoot object will move with the platform. A way around parenting and messing up the scale of the projectile.
			this.transform.position = _objectToFollow.transform.position;
		}

	}
}
