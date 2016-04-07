﻿using UnityEngine;
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

                    this.transform.position = currentSpawn._shootPoint.transform.position;

                    GameObject monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this.transform.position, this.transform.rotation);
                    monster.transform.rotation = this.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._target.transform.position - this.transform.position).normalized);

                    yield return new WaitForSeconds(currentSpawn._timeToNextSpawn);
                }

                yield return new WaitForSeconds(currentWave._timeToNextWave);
            }
            LevelManager.Instance.FinishLevel();
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
    }
}