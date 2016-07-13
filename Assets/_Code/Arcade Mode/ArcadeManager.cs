using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BounceDudes{

	public class ArcadeManager : MonoBehaviour {

		protected ArcadeWave _arcadeWave;

		public GameObject _spawner;
		public float _startDelay = 1f;
		public Character _last;

		public int _dead = 0;
		public int _maxEnemies;

		protected IEnumerator RunWave(List<Wave> waves)
		{
			for (int i = 0; i < waves.Count; i++)
			{
				Wave currentWave = waves[i];
				List<SpawnOption> currentSpaws = currentWave._spawns;

				LevelManager.Instance.CallWaveText (i+1);

				yield return new WaitForSeconds (2.0f); // Wait for wave text animation

				for (int j = 0; j < currentSpaws.Count; j++)
				{
					SpawnOption currentSpawn = currentSpaws[j];
					this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;
					this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._target.transform.position - this._spawner.transform.position).normalized);

					var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);

					var character = monster.GetComponent<Character>();
					character.Shoot();

					character.OnDie += this.OnLastDie;

					yield return new WaitForSeconds(currentSpawn._timeToNextSpawn);
				}

				yield return new WaitForSeconds(currentWave._timeToNextWave);
			}
		}

		public void StartNextWave()
		{
			this.StartCoroutine(this.RunWave(_arcadeWave.NextWave));
		}

		public IEnumerator WaitSeconds(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}

		public void OnLastDie(Character last)
		{
			last.OnDie -= this.OnLastDie;
			_dead++;
			if (_dead == _maxEnemies)
				LevelManager.Instance.FinishLevel();
		}
	}
}