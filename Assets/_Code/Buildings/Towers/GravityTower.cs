using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// A tower which power if a gravitational pull.
    /// </summary>
    public class GravityTower : Tower
    {
        public float _gravityForce = 0.05f;

        override public void AffectObject(GameObject affected)
        {
            affected.GetComponent<Rigidbody2D>().AddForce((this.transform.position - affected.transform.position).normalized * this._gravityForce, ForceMode2D.Force);
        }

    }
}
