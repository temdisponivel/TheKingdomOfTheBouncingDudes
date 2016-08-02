using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BounceDudes{

	[Serializable]
	public class ArcadeManager : MonoBehaviour {

		public static ArcadeManager Instance;

		// Saving this for performance improvements
		protected List<Wave> _nextFiveArcadeWaves = null;
		protected int _arcadeWaveIndex = 0;

		protected Wave _currentArcadeWave = null;
		public Wave CurrentArcadeWave { get { return _currentArcadeWave; } set { _currentArcadeWave = value; } }

		protected int _arcadePoints = 0;
		public int ArcadePoints { get { return _arcadePoints; } set { _arcadePoints = value; } }

		[Header("Wave Factory")]
		public List<GameObject> _allLv1Monsters;
		public List<GameObject> _allLv2Monsters;
		protected List<List<GameObject>> _allMonsters;
		public List<GameObject> _spawnPoints;
		public List<GameObject> _targetPoints;
		protected float _minIntervalTime = 0.6f;
		protected float _maxIntervalTime = 5.4f;
		protected float _intervalTimeBkp = 0f;
		protected int _minMonsterLevel = 0;
		protected int _maxMonsterLevel = 0;
		protected int _maxMonsterQuantity = 2;
		protected int _monsterQuantityBkp = 0;
		protected int _intervalPseudoChanceModifier = 0;
		protected int _intervalMaxPseudoChance = 3;
		protected int _waveNumber = 0;

		public GameObject _spawner;

		protected int _waveEnemiesDefeated = 0;
		protected int _waveEnemiesCount;


		void Awake(){
			Instance = this;
			CurrentArcadeWave = new Wave ();

			// Populate All Monsters
			_allMonsters = new List<List<GameObject>>();
			_allMonsters.Add(_allLv1Monsters);
			_allMonsters.Add(_allLv2Monsters);
		}

		void Start(){
			_intervalTimeBkp = _maxIntervalTime;
			_monsterQuantityBkp = _maxMonsterQuantity;
			AddArcadePoints (0); // Init the Point Text
			this.StartCoroutine(this.WaitSeconds(0.1f, this.StartNextArcadeWave));
		}


		public void AddArcadePoints(int value){
			this.ArcadePoints += value;
			ArcadeUIController.Instance.UpdateScoreText (this.ArcadePoints);
		}



		protected IEnumerator RunWave()
		{
			List<SpawnOption> currentSpaws = CurrentArcadeWave._spawns;

			LevelManager.Instance.CallWaveText (_waveNumber);

			yield return new WaitForSeconds (2.0f); // Wait for wave text animation

			for (int j = 0; j < currentSpaws.Count; j++)
			{
				SpawnOption currentSpawn = currentSpaws[j];
				this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;
				this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._targetPoint.transform.position - this._spawner.transform.position).normalized);

				var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);

				var character = monster.GetComponent<Character>();
				//character.Shoot(); // Call this on Monster class

				character.OnDie += this.OnLastDie;

				yield return new WaitForSeconds(currentSpawn._timeToNextSpawn);
			}
				
			//this.StartNextArcadeWave ();
		}

		public void OnLastDie(Character last)
		{
			last.OnDie -= this.OnLastDie;
			_waveEnemiesDefeated++;
			if (_waveEnemiesDefeated == _waveEnemiesCount) {
				_waveEnemiesDefeated = 0;
				StartNextArcadeWave ();
			}

		}

		public void StartNextArcadeWave()
		{
			this.MakeWave ();
			this.StartCoroutine(this.RunWave());
		}

		protected void MakeWave(){

			CurrentArcadeWave._spawns.Clear ();

			_waveNumber++;

			if (_waveNumber % 5 == 0 && _waveNumber != 1) {
				// Reduce the pseudo max chance, improving the rate of occurrence.
				_intervalMaxPseudoChance -= (int)(_intervalMaxPseudoChance * 0.3f); // -30%
				_intervalMaxPseudoChance = Mathf.Clamp (_intervalMaxPseudoChance, 1, 3);

				_maxMonsterQuantity++;

				_maxIntervalTime -= 0.7f;
				_maxIntervalTime = Mathf.Clamp(_maxIntervalTime, _minIntervalTime * 2f, _intervalTimeBkp);

				// ASSAULT!
				_intervalTimeBkp = _maxIntervalTime;
				_maxIntervalTime = _minIntervalTime;

				_monsterQuantityBkp = _maxMonsterQuantity;
				_maxMonsterQuantity *= 2;

				Debug.Log ("Increase!");
			}

			if (_waveNumber % 10 == 0 && _waveNumber != 1) {
				// BOB SHOWS UP!
			}

			if (_waveNumber >= 20) {
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
						Debug.Log ("Chubbyfy");
					}
				}

				// Creates the pseudo chance of spawning the next monster in a shorter period of time.
				if (Random.Range (_intervalPseudoChanceModifier, _intervalMaxPseudoChance+1) == _intervalMaxPseudoChance) {
					_intervalPseudoChanceModifier = 0;
					pickedIntervalTime = _minIntervalTime / 2f;
					Debug.Log ("Double!");
				}
				else {
					_intervalPseudoChanceModifier++;
				}

				SpawnOption newSpawn = new SpawnOption (pickedMonster, pickedSpawnPoint, pickedTargetPoint, pickedIntervalTime);

				this.CurrentArcadeWave._spawns.Add (newSpawn);
			}
		
			this._maxIntervalTime = this._intervalTimeBkp;
			this._maxMonsterQuantity = this._monsterQuantityBkp;
			this._waveEnemiesCount = this.CurrentArcadeWave._spawns.Count;
		}
			

		public IEnumerator WaitSeconds(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}
			
	}
}