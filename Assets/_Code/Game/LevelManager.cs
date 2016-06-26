using System;
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

        public GameObject _colliderPanel = null;
        public PausePanel _pausePanel = null;
        public LooseLevelPanel _loosePanel = null;
        public WinLevelPanel _winPanel = null;

        public GameObject _shownPositionPanels = null;
        public GameObject _hidenPositionPanels = null;

        public int EnemiesKilled { get; set; }

        public bool _showPausePanel = false;

        public bool WinPanelShown = false;
        public bool LoosePanelShown = false;
        public bool PausePanelShown = false;

        public void Awake()
        {
			this._uiCamera.aspect = GameManager.Instance.AspectRatio;	
            LevelManager._instance = this;
            this._baseHpBakp = this._playerBase.HP;
            GameManager.Instance.OnStateChange += this.StateChangeCallback;
        }

		public void Start(){
			if (GameManager.Instance.CurrentLevel.Id != LevelId.FIFTEEN) // If not the Boss Level
				AudioManager.Instance.PlayMusic(2);
		}
			
        public void GameOver()
        {
            this.EndLevel(false);
        }

        public void FinishLevel()
        {
            this.EndLevel(true);
        }

        protected void EndLevel(bool win)
        {
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
            
            if (win)
            {
				AudioManager.Instance.PlaySound(2, 0);
                this._winPanel.UpdateInfo(info);
                this._winPanel.Show();
                GameManager.Instance.AddLevelInfo(GameManager.Instance.CurrentLevel.Id, info);
            }
            else
            {
				AudioManager.Instance.PlaySound(2, 1);
				this._loosePanel.UpdateInfo (info);
                this._loosePanel.Show();
            }
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

        public void PauseGame()
        {
            GameManager.Instance.State = GameState.PAUSED;
        }

        public void UnpauseGame()
        {
            GameManager.Instance.State = GameState.PLAYING;
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
            /*
            this.GameOver();
             */
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

			AudioManager.Instance.PlayMusic (2);

			this.UnpauseGame();

            this.Dispose();
            //SceneManager.LoadScene(GameManager.Instance.LastLevel.SceneName);
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
    }
}