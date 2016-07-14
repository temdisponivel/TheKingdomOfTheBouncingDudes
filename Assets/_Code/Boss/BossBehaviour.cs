using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace BounceDudes
{

    /// <summary>
    /// Class that holds the behaviour of the boss of the game.s
    /// </summary>
    public class BossBehaviour : MonoBehaviour
    {

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
		protected Tweener _bossTween;

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
		public float _waveInterval = 5;
		protected float _waveTimer;
		protected float _waveSpawnModifier = 0;


        public int BossHP
        {
            get { return _hp; }
            set
            {
                _hp = value;
                if (_hp == 0)
                {
                    LevelManager.Instance.FinishLevel();
                }
                else
                    this.GetHit();
            }
        }

        public bool CanTakeHit = true;

        // Use this for initialization
        void Start()
        {
            this._bossAnimator = this.GetComponent<Animator>();
            this._bossFaceSprite = this._bossFace.GetComponent<SpriteRenderer>();
			this._bossTween = null;

            this._firstHpThreshold = (int)(this._hp * 0.7f);
            this._secondHpThreshold = (int)(this._hp * 0.5f);
            this._thirdHpThreshold = (int)(this._hp * 0.2f);

			this._waveTimer = this._waveInterval;

            this.MoveToDestination();

            AudioManager.Instance.PlayMusic(3);
        }

        // Update is called once per frame
        void Update()
        {
			if (_bossTween != null && !_bossTween.IsComplete()) {
				if (GameManager.Instance.State == Assets.Code.Game.GameState.PAUSED) {
					if (_bossTween.IsPlaying()) {
						_bossTween.Pause();
					}
				} else {
					if (!_bossTween.IsPlaying()) {
						_bossTween.Play();
					}
					this._waveTimer -= Time.deltaTime;
					if (this._waveTimer <= 0) {
						this._waveTimer = this._waveInterval - this._waveSpawnModifier;
						this.SpawnNextMonster ();
					}
				}
			}
				
        }

        protected void MoveToDestination()
        {
            foreach (var anim in this._ratArmyAnimators)
            {
                anim.SetBool("Walking", true);
            }

			_bossTween = this.transform.DOMove(this._bossDestination.transform.position, this._timeToDestination);
			_bossTween.OnComplete(this.OnMoveComplete);
			_bossTween.SetEase(Ease.Linear);
        }

        protected void OnMoveComplete()
        {

            // START GAME OVER ANIMATION
            // PAUSE PLAYER ACTIONS

            foreach (var anim in this._ratArmyAnimators)
            {
                anim.SetBool("Walking", false);
            }

            // SOUND: Stop Music and Play BOB Voice!
			AudioManager.Instance.PauseAllSounds ();
			AudioManager.Instance.PlayCharacterSound (1);
			this._bossBody.transform.DOMove(this._basePosition.transform.position, 0.5f).OnComplete(this.GameOverComplete).SetEase(Ease.InBack);
			_bossTween = null;
        }

        protected void GameOverComplete()
        {
            this._blackOut.enabled = true;
			AudioManager.Instance.PlayCharacterSound (0);

            LevelManager.Instance.GameOver();
        }

        protected void GetHit()
        {
            if (!CanTakeHit)
                return;

            this._hitCount++;
            this._bossFaceSprite.sprite = this._faceHit;

			AudioManager.Instance.PlayMonsterSound (13);
			AudioManager.Instance.PlayInterfaceSound (6);
            //CanTakeHit = true;
            this.StartCoroutine(this.WaitForAndCall(1f, () =>
            {
                CanTakeHit = true;
                SetToCurrentState();

            }));

			if (this._hitCount >= this._hitSpawnThreshold)
            {
                this.SpawnNextMonster();
                this._hitCount = 0;
            }
        }

        protected void SpawnNextMonster()
        {

            // If wave is over, repeat!
            if (this._nextSpawnIndex >= this._bossWave.Length)
                this._nextSpawnIndex = 0;

            SpawnOption currentSpawn = this._bossWave[this._nextSpawnIndex];

            this._spawner.transform.position = currentSpawn._spawnPoint.transform.position;
            this._spawner.transform.rotation = Quaternion.LookRotation(Vector3.forward, (currentSpawn._targetPoint.transform.position - this._spawner.transform.position).normalized);

            var monster = (GameObject)GameObject.Instantiate(currentSpawn._toSpawn, this._spawner.transform.position, this._spawner.transform.rotation);
            var character = monster.GetComponent<Character>();
            character.Shoot();

            this._nextSpawnIndex++;
        }

        protected void SetToCurrentState()
        {
			if (_hp >= _firstHpThreshold)
			{
				this._bossFaceSprite.sprite = this._faceNormal;
			}
            else if (_hp <= _firstHpThreshold && _hp > _secondHpThreshold)
            {
				this._bossFaceSprite.sprite = this._faceNormal;
				this._waveSpawnModifier = this._waveInterval * 0.15f; // 15% less seconds to spawn another monster
            }
            else if (_hp <= _secondHpThreshold && _hp > _thirdHpThreshold)
            {
				AudioManager.Instance.PlayMonsterSound (12);
                this._bossFaceSprite.sprite = this._faceAngry;
                this._bossAnimator.SetBool("Angry", true);
                this._bossArmAngry.GetComponent<SpriteRenderer>().enabled = true;
                this._bossArmNormal.GetComponent<SpriteRenderer>().enabled = false;

				this._waveSpawnModifier = this._waveInterval * 0.25f; // 25% less seconds to spawn another monster
            }
            else if (_hp <= _thirdHpThreshold)
            {
                this._bossFaceSprite.sprite = this._faceCry;
                this._bossAnimator.SetBool("Angry", false);
                this._bossArmAngry.GetComponent<SpriteRenderer>().enabled = false;
                this._bossArmNormal.GetComponent<SpriteRenderer>().enabled = true;

				this._waveSpawnModifier = this._waveInterval * 0.5f; // 50% less seconds to spawn another monster
            }
        }

        public IEnumerator WaitForAndCall(float seconds, Action callback)
        {

            yield return new WaitForSeconds(seconds);
            if (callback != null)
                callback();
        }

    }

}
