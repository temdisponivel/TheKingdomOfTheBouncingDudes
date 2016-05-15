using UnityEngine;
using System.Collections;

public class ShadowEffect : MonoBehaviour {

	public float _offsetY;
	protected Quaternion _trueRotation;


	// Use this for initialization
	void Start () {
		_trueRotation = new Quaternion(0f, 0f, 0f, 1.0f);
	}
		
	// Update is called once per frame
	void LateUpdate () {
		this.transform.rotation = _trueRotation;

		Vector3 p = this.transform.parent.position;
		Vector3 truePosition = new Vector3 (p.x, p.y + _offsetY, p.z);
		this.transform.position = truePosition;


	}
}
