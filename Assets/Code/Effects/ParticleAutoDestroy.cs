using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {

	protected ParticleSystem _particleSystem = null;

	// Use this for initialization
	void Start () {
		_particleSystem = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_particleSystem.time >= _particleSystem.duration){
			Destroy (this.gameObject);
		}

	}
}
