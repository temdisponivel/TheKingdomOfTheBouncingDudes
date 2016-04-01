using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that holds useful information about a spawn.
    /// </summary>
    [Serializable]
    public class SpawnOption
    {
        public GameObject _objectToSpawn = null;
        public float _timeToNextSpawn = 1f;
    }
}
