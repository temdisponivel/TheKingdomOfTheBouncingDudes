using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    public class HPTimer : MonoBehaviour
    {
        public float MaxSizeX = 0;
        public float MaxHP = 0;

		protected Vector2 _delta;
        protected Vector3 _scale;

        public bool BossHP = false;
        public BossBehaviour Boss;

        public void Start()
        {

			//this.GetComponent<RectTransform>().sizeDelta = new Vector2()
			//this.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._size + .15f, y = delta.y };
			if (this.BossHP)
            {
                _scale = this.transform.localScale;
                this.MaxSizeX = _scale.x;
                this.MaxHP = Boss.BossHP;
            }
            else
            {
                _delta = this.GetComponent<RectTransform>().sizeDelta;
                this.MaxSizeX = _delta.x;
                this.MaxHP = LevelManager.Instance._playerBase.HP;
            }
        }

        public void LateUpdate()
        {
            var hp = 0;
            if (this.BossHP)
            {
                hp = Boss.BossHP;
                var deltaAux = new Vector3
                {
                    x = MaxSizeX * (hp / this.MaxHP),
                    y = _scale.y,
                    z = _scale.z,
                };


                if (deltaAux.x <= MaxSizeX)
                    this.transform.localScale = deltaAux;
            }
            else
            {
                hp = LevelManager.Instance._playerBase.HP;
                var deltaAux = new Vector2
                {
                    x = MaxSizeX * (hp / this.MaxHP),
                    y = _delta.y
                };


                if (deltaAux.x <= MaxSizeX)
                    this.GetComponent<RectTransform>().sizeDelta = deltaAux;
            }
        }
    }
}
