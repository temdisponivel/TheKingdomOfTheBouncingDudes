using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

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

		[FormerlySerializedAs("_target")]
        public GameObject _targetPoint = null;
        public float _timeToNextSpawn = 1f;

		public SpawnOption(GameObject whoToSpawn, GameObject birthPoint, GameObject targetPoint, float timeToNext){
			_toSpawn = whoToSpawn;
			_spawnPoint = birthPoint;
			_targetPoint = targetPoint;
			_timeToNextSpawn = timeToNext;
		} 
    }
}
