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

        public void Start()
        {
            this.MaxSizeX = this.transform.localScale.x;
            this.MaxHP = LevelManager.Instance._playerBase.HP;
        }

        public void LateUpdate()
        {
            var newScale = new Vector3()
            {
                x = MaxSizeX * (LevelManager.Instance._playerBase.HP / this.MaxHP),
                y = this.transform.localScale.y,
                z = this.transform.localScale.z
            };

            if (newScale.x <= MaxSizeX)
                this.transform.localScale = newScale;
        }
    }
}
