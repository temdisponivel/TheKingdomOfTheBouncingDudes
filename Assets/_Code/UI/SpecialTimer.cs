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
        public float SpecialDuration = 0;

        public bool IsInSpecial = false;

        public void Start()
        {
            this.MaxSizeX = this.transform.localScale.x;
            this.CoolDownSpecial = Weapon.Instance._coolDownBetweenSpecials;
            this.SpecialDuration = Weapon.Instance._specialDuration;
        }

        public void LateUpdate()
        {
            bool up = true;
            if (IsInSpecial)
            {
                IsInSpecial = Weapon.Instance._special;

                if (!IsInSpecial)
                {
                    up = true;
                }
                else
                {
                    up = false;
                }
            }
            
            if (up)
            {
                var newScale = new Vector3()
                {
                    x = MaxSizeX * ((Time.time - Weapon.Instance._specialStartTime) / CoolDownSpecial),
                    y = this.transform.localScale.y,
                    z = this.transform.localScale.z
                };

                if (newScale.x <= MaxSizeX && newScale.x >= 0)
                    this.transform.localScale = newScale;
            }
            else
            {
                Debug.Log("FALLLLSEEEEE");
                Debug.Log(MaxSizeX - (MaxSizeX * ((Time.time - Weapon.Instance._specialStartTime) / SpecialDuration)));
                var newScale = new Vector3()
                {
                    x = MaxSizeX - (MaxSizeX * ((Time.time - Weapon.Instance._specialStartTime) / SpecialDuration)),
                    y = this.transform.localScale.y,
                    z = this.transform.localScale.z
                };

                if (newScale.x <= MaxSizeX && newScale.x >= 0)
                    this.transform.localScale = newScale;
            }
        }
    }
}
