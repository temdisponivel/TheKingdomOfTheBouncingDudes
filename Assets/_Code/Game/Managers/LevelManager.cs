﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Game;
using DG.Tweening;
using Assets._Code.Game;


namespace BounceDudes
{
    /// <summary>
    /// Class that manage usefull information about level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        static protected LevelManager _instance = null;
        static public LevelManager Instance { get { return LevelManager._instance; } }

		public Camera _uiCamera = null;

        public Base _playerBase = null;
        public Base _enemyBase = null;

        public float _baseHpBakp { get; set; }

        public Text _soldierNameText;

		[Header("Wave Text")]
		public Text _currentWaveText;
		public GameObject _waveTextInitialPosition;
		public GameObject _waveTextMiddlePosition;
		public GameObject _waveTextWaitingPosition;
		public GameObject _waveTextFinalPosition;

		[Header("Panels")]
        public GameObject _colliderPanel = null;
        public PausePanel _pausePanel = null;
        public LooseLevelPanel _loosePanel = null;
        public WinLevelPanel _winPanel = null;

        public GameObject _shownPositionPanels = null;
        public GameObject _hidenPositionPanels = null;

		[Header("Pause Panel")]
		public Text _challengeOneText;
		public Text _challengeTwoText;
		public Text _challengeThreeText;
		protected bool _updatedOneTime = false;
		protected Tween _panelIdle;

		[Header("Spawn Points")]
		public List<GameObject> _spawnPoints;
		public float _spawnPointsTrueY;

        public int EnemiesKilled { get; set; }

        public bool _showPausePanel = false;

        public bool WinPanelShown = false;
        public bool LoosePanelShown = false;
        public bool PausePanelShown = false;

		protected bool _wonLostScreenON = false;


        public void Awake()
        {
			Camera.main.aspect = GameManager.Instance.AspectRatio;
			this._uiCamera.aspect = GameManager.Instance.AspectRatio;	

            LevelManager._instance = this;
            this._baseHpBakp = this._playerBase.HP;

            GameManager.Instance.OnStateChange += this.StateChangeCallback;

        }

		public void Start(){
			if (GameManager.Instance.CurrentLevel.Id != LevelId.FIFTEEN) // If not the Boss Level
				AudioManager.Instance.PlayMusic(2);

			if (_spawnPoints[0] != null) {
				foreach (GameObject sp in _spawnPoints) {
					sp.transform.position = new Vector3 (sp.transform.position.x, _spawnPointsTrueY, 0f);
				}
			}

			if (GameManager.Instance.NextLevelSoldiersDefinition.Count == 1)
				#if UNITY_ANDROID
				GameManager.GPManagerInstance.UnlockAchievement (GPGSIds.achievement_one_man_army);
				#endif

			this.UnpauseGame();
		}


        public void GameOver()
        {
			if (!_wonLostScreenON)
            	this.EndLevel(false);
        }

        public void FinishLevel()
        {
			if (!_wonLostScreenON)
	        	this.EndLevel(true);
        }

        protected void EndLevel(bool win)
        {

			_wonLostScreenON = true;

			if (GameManager.Instance.CurrentLevel.Id != LevelId.FIFTEEN) // If not the Boss Level
				AudioManager.Instance.StopMusic (2);
			else
				AudioManager.Instance.StopMusic (3);

			var bkpTimeScale = Time.timeScale;
			var newTimeScale = 0.6f;
			var waitTime = 1.0f;

			Tweener auxTween = DOTween.To (() => Time.timeScale, x => Time.timeScale = x, newTimeScale, waitTime);
			auxTween.OnComplete (() => {
				Time.timeScale = bkpTimeScale;
			});
			auxTween.SetEase (Ease.OutExpo);
			auxTween.SetId ("TimeScale");


			this.StartCoroutine(this.WaitForAndCall(waitTime, () =>
			{
				Time.timeScale = bkpTimeScale;

				AudioManager.Instance.PauseAllSounds ();

	        	LevelInformation info = new LevelInformation();
	            info.EnemiesKilled = this.EnemiesKilled;
	            info.Finished = win;
	            info.ShootCount = Weapon.Instance.ShootCount;
	            info.LevelId = GameManager.Instance.CurrentLevel.Id;

	            int biggest = 0;
	            for (int i = 0; i < GameManager.Instance.StarsPercent.Count; i++)
	            {
	                if (this._playerBase.HP/this._baseHpBakp >= GameManager.Instance.StarsPercent[i])
	                    biggest = i + 1;
	            }

	            info.Star = biggest;

	            Dictionary<Challenge, int[]> soldiersEarned = new Dictionary<Challenge, int[]>();

	            var challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackOne;
	            if (ChallengeManager.ValidateCompletion(challenge._challenge))
	            {
	                soldiersEarned.Add(challenge._challenge, challenge._soldierToGive);
	            }

	            challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackTwo;
	            if (ChallengeManager.ValidateCompletion(challenge._challenge))
	            {
	                soldiersEarned.Add(challenge._challenge, challenge._soldierToGive);
	            }

	            challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackThree;
	            if (ChallengeManager.ValidateCompletion(challenge._challenge))
	            {
	                soldiersEarned.Add(challenge._challenge, challenge._soldierToGive);
	            }
					

	            info.ChallengesCompleted = soldiersEarned;
	            
				GameObject panelToIdle;


	            if (win)
	            {
					#if UNITY_ANDROID
					if (GameManager.Instance.NextLevelSoldiersDefinition.Count == 1)
						GameManager.GPManagerInstance.UnlockAchievement (GPGSIds.achievement_true_one_man_army);
					#endif
					
					AudioManager.Instance.PlayFanfareSound (0);
	                this._winPanel.UpdateInfo(info);
	                this._winPanel.Show();
	                GameManager.Instance.AddLevelInfo(GameManager.Instance.CurrentLevel.Id, info);

					panelToIdle = this._winPanel.gameObject;
	            }
	            else
	            {
					AudioManager.Instance.PlayFanfareSound (1);
					this._loosePanel.UpdateInfo (info);
	                this._loosePanel.Show();

					panelToIdle = this._loosePanel.gameObject;
	            }

				_panelIdle = panelToIdle.transform.DOMoveY (panelToIdle.transform.position.y - 0.07f, 1.0f).SetEase(Ease.InOutCubic).SetLoops (-1, LoopType.Yoyo).SetId("Idle");
				
				#if UNITY_ANDROID
				GameManager.GPManagerInstance.CommitMonstersDefeated (this.EnemiesKilled);
				GameManager.GPManagerInstance.CheckFullComplete();
				GameManager.GPManagerInstance.CheckKingTruePower();
				#endif
				
			}));
        }

        public void KillEnemy(Character enemy)
        {
            this.EnemiesKilled++;
        }

        public void StateChangeCallback()
        {

			if (GameManager.Instance.State == GameState.PAUSED)
			{
				if (!WinPanelShown && !LoosePanelShown)
					this._pausePanel.Show();
			}
			else
			{
				if (PausePanelShown)
					this._pausePanel.Hide();
			}
				
        }

		public IEnumerator WaitForAndCall(float seconds, Action callback)
		{

			yield return new WaitForSeconds(seconds);
			if (callback != null)
				callback();
		}


        public void PauseGame()
        {
			DOTween.Pause ("Transform");
            GameManager.Instance.State = GameState.PAUSED;
			this.PausePanelUpdateInfo ();
        }

        public void UnpauseGame()
        {
			DOTween.Play ("Transform");
            GameManager.Instance.State = GameState.PLAYING;
        }

		public void InCommonButtonActions(){
			AudioManager.Instance.PlayInterfaceSound (0);
			_wonLostScreenON = false;
		}

		public void ButtonPause()
		{
			this.PauseGame ();
			AudioManager.Instance.PlayInterfaceSound (0);
		}

		public void ButtonContinue()
		{
			this.UnpauseGame ();
			this.InCommonButtonActions ();
		}

		public void ButtonQuit()
		{
			this.Quit ();
			this.InCommonButtonActions ();
		}

		public void ButtonRetry()
		{
			this.PlayAgain ();
			this.InCommonButtonActions ();
		}
			
        public void Quit()
        {

            if (WinPanelShown)
                _winPanel.Hide();
			
            else if (LoosePanelShown)
                _loosePanel.Hide();
			
            else if (PausePanelShown)
                _pausePanel.Hide();

            GameManager.Instance.LoadScene("MapMenu");
			AudioManager.Instance.PlayMusic (1);

			this.UnpauseGame();
            this.Dispose();
	
        }

        public void PlayAgain()
        {
            
            if (WinPanelShown)
                _winPanel.Hide();
			
            else if (LoosePanelShown)
                _loosePanel.Hide();
			
            else if (PausePanelShown)
                _pausePanel.Hide();
			
            GameManager.Instance.LoadScene(GameManager.Instance.CurrentLevel.SceneName);
            this.UnpauseGame();
            this.Dispose();

        }

        public void Dispose()
        {
            GameManager.Instance.OnStateChange -= this.StateChangeCallback;
        }

        void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
                this.PauseGame();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                this.PauseGame();
        }

		public void FadeOutCollider(Action callback)
		{
			var image = this._colliderPanel.GetComponent<Image>();
			Color color = image.color;
			color.a = 0;
			image.DOBlendableColor(color, .5f).OnComplete(() =>
				{
					if (callback != null)
						callback();
					this._colliderPanel.SetActive(false);
				});
		}

		public void FadeInCollider(Action callback)
		{
			this._colliderPanel.SetActive(true);
			var image = this._colliderPanel.GetComponent<Image>();
			Color color = image.color;
			color.a = .7f;
			image.DOBlendableColor(color, .5f).OnComplete(() =>
				{
					if (callback != null)
						callback();
				});
		}


		public void PausePanelUpdateInfo()
		{
			Level level = GameManager.Instance.CurrentLevel;

			var redColor = new Color (221f/255f, 39f/255f, 39f/255f, 1);
			var greenColor = new Color (62f/255f, 180f/255f, 63f/255f, 1);

			var challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackOne;
			if (ChallengeManager.ValidateCompletionImpossible(challenge._challenge))
				_challengeOneText.color = redColor;
			else
				if (ChallengeManager.ValidateCompletion(challenge._challenge)) 
					_challengeOneText.color = greenColor;

			challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackTwo;
			if (ChallengeManager.ValidateCompletionImpossible(challenge._challenge))
				_challengeTwoText.color = redColor;
			else
				if (ChallengeManager.ValidateCompletion(challenge._challenge)) 
					_challengeTwoText.color = greenColor;

			challenge = GameManager.Instance.CurrentLevel.SoldiersByChallengeHackThree;
			if (ChallengeManager.ValidateCompletionImpossible (challenge._challenge))
				_challengeThreeText.color = redColor;
			else {
				if (ChallengeManager.ValidateCompletion(challenge._challenge)) 
					_challengeThreeText.color = greenColor;
			}

		

			Challenge currentChallenge = level.SoldiersByChallengeHackOne._challenge;
			if (!_updatedOneTime) {
				currentChallenge.SetDescription ();
				_challengeOneText.text = currentChallenge.Description;
				_challengeOneText.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);
			}

			currentChallenge = level.SoldiersByChallengeHackTwo._challenge;
			if (!_updatedOneTime) {
				currentChallenge.SetDescription ();
				_challengeTwoText.text = currentChallenge.Description;
				_challengeTwoText.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);
			}

			currentChallenge = level.SoldiersByChallengeHackThree._challenge;

			if (!_updatedOneTime) {
				currentChallenge.SetDescription ();
				_challengeThreeText.text = currentChallenge.Description;
				_challengeThreeText.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);
			}
				

			this._updatedOneTime = true;

		}
			

		public void CallWaveText(int waveIndex){

			_currentWaveText.GetComponent<TextToTraslate> ().TranslateWithOneArgument(waveIndex, true);


			_currentWaveText.transform.DOMove (_waveTextMiddlePosition.transform.position, 2.1f).SetEase(Ease.OutExpo).OnComplete(() => {
				_currentWaveText.transform.DOMove(_waveTextFinalPosition.transform.position, 0.7f).SetEase(Ease.InOutBack).OnComplete(() => {
					_currentWaveText.transform.position = _waveTextInitialPosition.transform.position;
				}).SetId("Transform");

			}).SetId("Transform");
				
		}


    }
}