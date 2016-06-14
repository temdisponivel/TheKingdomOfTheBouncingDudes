using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that holds useful information about a class.
    /// </summary>
    [Serializable]
    public class LevelInformation
    {
        public LevelId LevelId;
        public int EnemiesKilled { get; set; }
        public int ShootCount { get; set; }
        public bool Finished { get; set; }
        public Dictionary<Challenge, int[]> ChallengesCompleted { get; set; }
        public int Star { get; set; }
    }
}