using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BounceDudes{

	[Serializable]
	public class ArcadeManager : MonoBehaviour {

		public static ArcadeManager Instance;

		protected Wave _arcadeWave = null;
		protected Wave ArcadeWave { get { return _arcadeWave; } set { _arcadeWave = value; } }

		protected int _arcadePoints = 0;
		public int ArcadePoints { get { return _arcadePoints; } set { _arcadePoints = value; } }

		[Header("Wave Factory")]
		public List<GameObject> _allLv1Monsters;
		public List<GameObject> _allLv2Monsters;
		protected List<List<GameObject>> _allMonsters;
		public List<GameObject> _spawnPoints;
		public List<GameObject> _targetPoints;
		public float _minIntervalTime = 1.4f;
		public float _maxIntervalTime = 5.4f;
		protected int _minMonsterLevel = 0;
		protected int _maxMonsterLevel = 0;
		protected int _maxMonsterQuantity = 2;
		protected int _intervalPseudoChanceModifier = 0;
		protected int _intervalMaxPseudoChance = 3;
		protected int _waveNumber = 0;

		public GameObject _spawner;

		protected int _waveEnemiesDefeated = 0;
		protected int _waveEnemiesCount;


		void Awake(){
			Instance = this;
			ArcadeWave = new Wave (new List<SpawnOption> ());

			// Populate All Monsters
			_allMonsters = new List<List<GameObject>>();
			_allMonsters.Add(_allLv1Monsters);
			_allMonsters.Add (_allLv2Monsters);
		}

		void Start(){
			this.StartCoroutine(this.WaitSeconds(0.1f, this.StartNextArcadeWave));
		}


		public void AddArcadePoints(int value){
			
		}



		protected IEnumerator RunWave()
		{
			List<SpawnOption> currentSpaws = ArcadeWave._spawns;

			LevelManager.Instance.CallWaveText (_waveNumber);

			yield return new WaitForSeconds (2.0f); // Wait for wave text animation

			for (int j = 0; j < currentSpaws.Count; j++)
			{
				SpawnOption currentSpawn = currentSpaws[j];
				this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;
				this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._targetPoint.transform.position - this._spawner.transform.position).normalized);

				var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);

				var character = monster.GetComponent<Character>();
				character.Shoot();

				character.OnDie += this.OnLastDie;

				yield return new WaitForSeconds(currentSpawn._timeToNextSpawn);
			}
				
			this.StartNextArcadeWave ();
		}

		public void OnLastDie(Character last)
		{
			last.OnDie -= this.OnLastDie;
			_waveEnemiesDefeated++;
			if (_waveEnemiesDefeated == _waveEnemiesCount) {
			}
				//StartNextArcadeWave ();
		}

		public void StartNextArcadeWave()
		{
			this.MakeNewWave ();
			this.StartCoroutine(this.RunWave());
		}

		protected void MakeNewWave(){

			ArcadeWave._spawns.Clear ();

			_waveNumber++;

			if (_waveNumber % 5 == 0 && _waveNumber != 1) {
				// Reduce the pseudo max chance, improving the rate of occurrence.
				_intervalMaxPseudoChance -= (int)(_intervalMaxPseudoChance * 0.2f); // -20%
				_intervalMaxPseudoChance = Mathf.Clamp (_intervalMaxPseudoChance, 1, 3);
				_maxMonsterQuantity++;

				_maxIntervalTime -= 0.1f;
				_maxIntervalTime = Mathf.Clamp(_maxIntervalTime, _minIntervalTime, 5.4f);

				Debug.Log ("Increase!");
			}

			if (_waveNumber % 10 == 0 && _waveNumber != 1) {
				// BOB SHOWS UP!
			}

			if (_waveNumber >= 10) {
				_maxMonsterLevel = 1;
			}


			for (var i = 0; i < _maxMonsterQuantity; i++) {
				
				int pickedMonsterLevel = Random.Range (_minMonsterLevel, _maxMonsterLevel+1);
				List<GameObject> monsterListAux;
				monsterListAux = _allMonsters.ElementAt(pickedMonsterLevel);

				GameObject pickedMonster = monsterListAux [Random.Range (0, monsterListAux.Count)];
				GameObject pickedSpawnPoint = _spawnPoints [Random.Range (0, _spawnPoints.Count)];
				GameObject pickedTargetPoint = _targetPoints [Random.Range (0, _targetPoints.Count)];
				float pickedIntervalTime = Random.Range (_minIntervalTime, _maxIntervalTime);

				// If monster picked is not a CHUBBY, re-roll for a chance to change into a CHUBBY.
				if (pickedMonster != monsterListAux [0]) { 
					if (Random.Range (0, 3) == 2) { // 33%
						pickedMonster = monsterListAux [0];
						Debug.Log ("Chubbyply");
					}
				}

				// Creates the pseudo chance of spawning the next monster in a shorter period of time.
				if (Random.Range (_intervalPseudoChanceModifier, _intervalMaxPseudoChance+1) == _intervalMaxPseudoChance) {
					_intervalPseudoChanceModifier = 0;
					pickedIntervalTime = 0.7f;
					Debug.Log ("Double!");
				}
				else {
					_intervalPseudoChanceModifier++;
				}

				SpawnOption newSpawn = new SpawnOption (pickedMonster, pickedSpawnPoint, pickedTargetPoint, pickedIntervalTime);

				this.ArcadeWave._spawns.Add (newSpawn);
			}

			Debug.Log (this.ArcadeWave._spawns.Count);
			this._waveEnemiesCount = this.ArcadeWave._spawns.Count;
		}
			

		public IEnumerator WaitSeconds(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}
			
	}
}