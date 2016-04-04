using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BounceDudes
{
    /// <summary>
    /// Class that controls the wave.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        public List<Wave> _waves = null;
        public float _startDelay = 1f;
        
        public void Start()
        {
            this.StartCoroutine(this.WaitSeconds(this._startDelay, this.StartWave));
        }

        public IEnumerator ManageWave()
        {
            for (int i = 0; i < this._waves.Count; i++)
            {
                Wave currentWave = this._waves[i];
                List<SpawnOption> currentSpaws = currentWave._spawns;
                for (int j = 0; j < currentSpaws.Count; j++)
                {
                    SpawnOption currentSpawn = currentSpaws[j];
                    if (currentSpawn == null) { continue; }
                    currentSpawn._objectToSpawn.SetActive(true);
                    yield return new WaitForSeconds(currentSpawn._timeToNextSpawn);
                }
                yield return new WaitForSeconds(currentWave._timeToNextWave);
            }
        }

        public void StartWave()
        {
            this.StartCoroutine(this.ManageWave());
        }

        public IEnumerator WaitSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }
        /*
        public void OnDrawGizmos()
        {
            for (int i = 0; i < this._waves.Count; i++)
            {
                UnityEngine.Random.seed = i;
                Gizmos.color = UnityEngine.Random.ColorHSV();
                Wave currentWave = this._waves[i];
                if (currentWave == null) { continue; }
                List<SpawnOption> currentSpaws = currentWave._spawns;
                for (int j = 0; j < currentSpaws.Count - 1; j++)
                {
                    SpawnOption currentSpawn = currentSpaws[j];
                    if (currentSpawn == null) { continue; }
                    if (currentSpawn._objectToSpawn.transform.parent.gameObject.activeInHierarchy)
                    {
                        Gizmos.DrawLine(currentSpawn._objectToSpawn.transform.position, currentSpaws[j + 1]._objectToSpawn.transform.position);
                    }
                }
            }
        }
         */ 
    }
}