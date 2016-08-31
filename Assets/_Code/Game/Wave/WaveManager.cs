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
        
        public int _dead = 0;
        public int _maxEnemies;


        public void Start()
        {
            this._maxEnemies = this._waves.Sum(w => w._spawns.Count);
            this.StartCoroutine(this.WaitSeconds(this._startDelay, this.StartWave));
        }

        public IEnumerator ManageWave()
        {
            for (int i = 0; i < this._waves.Count; i++)
            {
                Wave currentWave = this._waves[i];
                List<SpawnOption> currentSpaws = currentWave._spawns;

				LevelManager.Instance.CallWaveText (i+1);

				yield return new WaitForSeconds (2.0f); // Wait for wave text animation

                for (int j = 0; j < currentSpaws.Count; j++)
                {
                    SpawnOption currentSpawn = currentSpaws[j];

					this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;

                    this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._targetPoint.transform.position - this._spawner.transform.position).normalized);

                    var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);

					var character = monster.GetComponent<Character> ();
                    //character.Shoot(); // WILL BE CALLED ON THE INTRO ANIMATION

					character.OnDie += this.OnDie;

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

        public void OnDie(Character charToDie)
        {
            charToDie.OnDie -= this.OnDie;
            _dead++;

            if (_dead == _maxEnemies)
				if (LevelManager.Instance._playerBase.HP >= 1)
               		LevelManager.Instance.FinishLevel();
        }
    }
}