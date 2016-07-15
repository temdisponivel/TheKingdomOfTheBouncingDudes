using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    public class SpecialTimer : MonoBehaviour
    {
        public float MaxSizeX = 0;
        public float SpecialMaxValue = 0;
        public float SpecialDuration = 0;

        public bool IsInSpecial = false;

        public void Start()
        {
            this.MaxSizeX = this.transform.localScale.x;
			this.SpecialMaxValue = Weapon.Instance.SpecialMaxValue;
        }

        public void LateUpdate()
        {
            var newScale = new Vector3()
            {
				x = MaxSizeX * Weapon.Instance.SpecialCurrentPoints / SpecialMaxValue,
                y = this.transform.localScale.y,
                z = this.transform.localScale.z
            };

            if (newScale.x <= MaxSizeX && newScale.x >= 0)
                this.transform.localScale = newScale;
        }
    }
}
