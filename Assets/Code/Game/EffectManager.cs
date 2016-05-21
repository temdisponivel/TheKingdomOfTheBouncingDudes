using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

	static protected EffectManager _instance = null;
	static public EffectManager Instance { get { return EffectManager._instance; } }

	[Header("Particle Effects")]
	public ParticleSystem _hitEffect = null;
	public ParticleSystem _smokeEffect = null;
	public ParticleSystem _dieEffect = null;
	public ParticleSystem _wallHitEffect = null;

	[Header("Object Effects")]
	public GameObject _shadowEffectPrefab = null;
	public GameObject _slimeEffectPrefab = null;

	// Use this for initialization
	void Start () {
		if (EffectManager.Instance == null)
		{
			EffectManager._instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			GameObject.Destroy(this.gameObject);
			return;
		}
	}
		
	public void CreateHitEffect(Transform target){
		this.CreateEffect (_hitEffect, target);
	}

	public void CreateSmokeEffect(Transform target){
		this.CreateEffect (_smokeEffect, target);
	}

	public void CreateDieEffect(Transform target){
		this.CreateEffect (_dieEffect, target);
	}

	public void CreateWallHitEffect(Transform target){
		this.CreateEffect (_wallHitEffect, target);
	}

	protected void CreateEffect(ParticleSystem particle, Transform target){
		Instantiate(particle, target.position, target.rotation);
	}
}
