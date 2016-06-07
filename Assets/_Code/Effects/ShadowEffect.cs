using UnityEngine;
using System.Collections;

namespace BounceDudes
{

    public class ShadowEffect : MonoBehaviour
    {

        public float _offsetY;
        public Vector2 _scale;

        protected Quaternion _trueRotation;
        protected GameObject _shadowObject = null;


        void Start()
        {
            _shadowObject = EffectManager.Instance.AttachShadowEffect(this.transform);
            _trueRotation = Quaternion.identity;
            this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        void LateUpdate()
        {

            Vector3 p = this.transform.position;
            Vector3 truePosition = new Vector3(p.x, p.y + _offsetY, p.z);
            this._shadowObject.transform.position = truePosition;

            this._shadowObject.transform.rotation = _trueRotation;

            this._shadowObject.transform.localScale = new Vector3(_scale.x, _scale.y, 1);

        }

    }

}
