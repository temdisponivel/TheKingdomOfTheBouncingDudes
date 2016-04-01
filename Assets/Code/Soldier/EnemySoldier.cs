using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a enemy soldier.
    /// </summary>
    public class EnemySoldier : Soldier
    {
        public override void Shoot()
        {
            this.RigidBody.AddForce(this.transform.up * this._velocity, ForceMode2D.Impulse);
        }
    }
}
