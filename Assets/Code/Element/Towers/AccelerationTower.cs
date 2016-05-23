using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    public class AccelerationTower : Tower
    {
        public float _acceleration = 9.8f;

        override public void AffectObject(GameObject affected)
        {
            Rigidbody2D rigidAffected = affected.GetComponent<Rigidbody2D>();
            rigidAffected.AddForce(rigidAffected.velocity.normalized * this._acceleration, ForceMode2D.Force);
        }
    }
}
