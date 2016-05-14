using UnityEngine;
using System.Collections;

public class ShadowEffect : MonoBehaviour {

	protected GameObject _character;
	protected Quaternion _trueRotation; 

	// Use this for initialization
	void Start () {
		_trueRotation = this.transform.rotation;
	}
		
	// Update is called once per frame
	void LateUpdate () {
		
		this.transform.rotation = _trueRotation;
	}
}
