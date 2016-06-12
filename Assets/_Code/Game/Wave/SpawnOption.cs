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
        public GameObject _toSpawn;
        public GameObject _spawnPoint = null;
        public GameObject _target = null;
        public float _timeToNextSpawn = 1f;
    }
}
