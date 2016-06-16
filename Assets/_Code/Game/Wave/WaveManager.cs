using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using JetBrains.Annotations;

namespace BounceDudes
{
    /// <summary>
    /// Class that controls the wave.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
		public GameObject _spawner;
        public List<Wave> _waves = null;
        public float _startDelay = 1f;
        public Character _last;
        
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

					this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;

                    this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._target.transform.position - this._spawner.transform.position).normalized);

                    var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);

                    var character = monster.GetComponent<Character>();
                    character.Shoot();

                    if (i == this._waves.Count - 1 && j == currentSpaws.Count - 1)
                    {
                        _last = character;
                        _last.OnDie += this.OnLastDie;
                    }

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

        public void OnLastDie(Character last)
        {
            _last.OnDie -= this.OnLastDie;
            LevelManager.Instance.FinishLevel();
        }
    }
}