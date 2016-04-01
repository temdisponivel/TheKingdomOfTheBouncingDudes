using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BounceDudes
{
    /// <summary>
    /// Class that represents a wave.
    /// </summary>
    [Serializable]
    public class Wave
    {
        public List<SpawnOption> _spawns = null;
        public float _timeToNextWave = 1f;
    }
}
