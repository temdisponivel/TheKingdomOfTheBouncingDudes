using UnityEngine;

namespace BounceDudes
{

    public class EffectManager : MonoBehaviour
    {

        static protected EffectManager _instance = null;
        static public EffectManager Instance { get { return EffectManager._instance; } }

        [Header("Particle Effects")]
        public ParticleSystem _hitEffect = null;
        public ParticleSystem _smokeEffect = null;
        public ParticleSystem _dieEffect = null;
        public ParticleSystem _wallHitEffect = null;
        public ParticleSystem _shinyEffect = null;

        [Header("Object Effects")]
        public GameObject _shadowEffect = null;
        public GameObject _slimeEffect = null;

        // Use this for initialization
        void Awake()
        {
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

        public void AttachShinyEffect(Transform target)
        {
            this.AttachEffect(_shinyEffect, target);
        }

        public GameObject AttachShadowEffect(Transform target)
        {
            return this.AttachEffectAndReturn(_shadowEffect, target);
        }

        public GameObject AttachSlimeEffect(Transform target)
        {
            return this.AttachEffectAndReturn(_slimeEffect, target);
        }


        protected void AttachEffect(ParticleSystem effect, Transform target)
        {
			ParticleSystem particleToAttach = ((ParticleSystem)GameObject.Instantiate(effect, target.position, target.rotation));
            particleToAttach.transform.parent = target;
            particleToAttach.transform.localScale = target.localScale;
        }

        protected GameObject AttachEffectAndReturn(GameObject effect, Transform target)
        {
            GameObject objectToAttach = ((GameObject)GameObject.Instantiate(effect, target.position, target.rotation));
            objectToAttach.transform.parent = target;
            return objectToAttach;
        }

        public void CreateHitEffect(Transform target)
        {
            this.CreateEffect(_hitEffect, target);
        }

        public void CreateSmokeEffect(Transform target)
        {
            this.CreateEffect(_smokeEffect, target);
        }

        public void CreateDieEffect(Transform target)
        {
            this.CreateEffect(_dieEffect, target);
        }

        public void CreateWallHitEffect(Transform target)
        {
            this.CreateEffect(_wallHitEffect, target);
        }

        protected void CreateEffect(ParticleSystem effect, Transform target)
        {
            GameObject.Instantiate(effect, target.position, target.rotation);
        }
    }
}