using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that holds useful information about a class.
    /// </summary>
    public class LevelInformation
    {
        public int EnemiesKilled { get; set; }
        public float Score { get; set; }
        public int ShootCount { get; set; }
        public bool Finished { get; set; }
        public bool EarnSoldier { get; set; }
        public int SoldierId { get; set; }
    }
}