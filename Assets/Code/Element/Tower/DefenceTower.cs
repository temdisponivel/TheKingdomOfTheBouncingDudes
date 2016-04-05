using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a defence tower.
    /// </summary>
    public class DefenceTower : Tower
    {
        public float _regenerationMultiplier = 2f;

        override public void AffectObject(GameObject affected)
        {
            affected.GetComponent<Character>().HP += this._regenerationMultiplier * Time.deltaTime;
        }
    }
}
