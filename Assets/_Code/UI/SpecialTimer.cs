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
        public float CoolDownSpecial = 0;

        public bool IsInSpecial = false;

        public void Start()
        {
            this.MaxSizeX = this.transform.localScale.x;
            this.CoolDownSpecial = Weapon.Instance._coolDownBetweenSpecials + Weapon.Instance._specialDuration;
        }

        public void LateUpdate()
        {
            var newScale = new Vector3()
            {
                x = MaxSizeX * ((Time.time - Weapon.Instance._specialStartTime) / CoolDownSpecial),
                y = this.transform.localScale.y,
                z = this.transform.localScale.z
            };

            if (newScale.x <= MaxSizeX)
                this.transform.localScale = newScale;
        }
    }
}
