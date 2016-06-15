using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace BounceDudes
{
	/// <summary>
	/// Class that holds the behaviour of the boss of the game.s
	/// </summary>
	public class BossBehaviour : MonoBehaviour {

		[Header("Boss Parts")]
		public GameObject _bossBody;
		public GameObject _bossArmAngry;
		public GameObject _bossArmNormal;
		public GameObject _bossLeg;
		public GameObject _bossFace;
		public Sprite _faceNormal;
		public Sprite _faceHit;
		public Sprite _faceAngry;
		public Sprite _faceCry;
		protected SpriteRenderer _bossFaceSprite;

		[Header("Animation")]
		protected Animator _bossAnimator;
		public Animator[] _ratArmyAnimators;
		public GameObject _bossDestination;
		public GameObject _basePosition;
		public Image _blackOut;

		[Header("Stats")]
		public int _hp = 50;
		public float _timeToDestination = 60.0f;
		public int _hitSpawnThreshold = 3;
		protected int _hitCount = 0;
		protected int _firstHpThreshold, _secondHpThreshold, _thirdHpThreshold;

		[Header("Wave")]
		public GameObject _spawner;
		public SpawnOption[] _bossWave;
		protected int _nextSpawnIndex = 0;


		public int BossHP { get { return _hp; } set { _hp = value; this.GetHit (); } }

		// Use this for initialization
		void Start () {
			this._bossAnimator = this.GetComponent<Animator> ();
			this._bossFaceSprite = this._bossFace.GetComponent<SpriteRenderer> ();

			this._firstHpThreshold  =  (int) (this._hp * 0.7f);
			this._secondHpThreshold =  (int) (this._hp * 0.4f);
			this._thirdHpThreshold  =  (int) (this._hp * 0.1f);

			this.MoveToDestination ();

		}
		
		// Update is called once per frame
		void Update () {
			SetToCurrentState ();
		}

		protected void MoveToDestination(){

			foreach (var anim in this._ratArmyAnimators) {
				anim.SetBool ("Walking", true);
			}

			this.transform.DOMove (this._bossDestination.transform.position, this._timeToDestination).OnComplete(this.OnMoveComplete);
		}

		protected void OnMoveComplete(){

			// START GAME OVER ANIMATION
			// PAUSE PLAYER ACTIONS

			foreach (var anim in this._ratArmyAnimators) {
				anim.SetBool ("Walking", false);
			}

			// SOUND: Stop Music and Play BOB Voice!
			this._bossBody.transform.DOMove (this._basePosition.transform.position, 0.5f).OnComplete(this.GameOverComplete);

		}

		protected void GameOverComplete(){
			this._blackOut.enabled = true;

			LevelManager.Instance.GameOver ();
		}

		protected void GetHit(){
			
			this._hitCount++;

			this._bossFaceSprite.sprite = this._faceHit; // TODO: WaitForXSeconds then call "SetToCurrentState()" (1~1.5 seconds max)

			if (this._hitCount >= this._hitSpawnThreshold) {
				this.SpawnNextMonster ();
				this._hitCount = 0;
			}
		}

		protected void SpawnNextMonster(){

			// If wave is over, repeat!
			if (this._nextSpawnIndex >= this._bossWave.Length)
				this._nextSpawnIndex = 0;

			SpawnOption currentSpawn = this._bossWave [this._nextSpawnIndex];

			this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;
			this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._target.transform.position - this._spawner.transform.position).normalized);

			var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);
			var character = monster.GetComponent<Character>();
			character.Shoot();

			this._nextSpawnIndex++;
		}

		protected void SetToCurrentState(){
			if (_hp <= _firstHpThreshold && _hp > _secondHpThreshold) {
				this._bossFaceSprite.sprite = this._faceNormal;
			}
			else if (_hp <= _secondHpThreshold && _hp > _thirdHpThreshold){
				this._bossFaceSprite.sprite = this._faceAngry;
				this._bossAnimator.SetBool ("Angry", true);
				this._bossArmAngry.GetComponent<SpriteRenderer> ().enabled = true;
				this._bossArmNormal.GetComponent<SpriteRenderer> ().enabled = false;
			}
			else if (_hp <= _thirdHpThreshold){
				this._bossFaceSprite.sprite = this._faceCry;
				this._bossAnimator.SetBool ("Angry", false);
				this._bossArmAngry.GetComponent<SpriteRenderer> ().enabled = false;
				this._bossArmNormal.GetComponent<SpriteRenderer> ().enabled = true;
			}
		}

	}
}
