using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Game;
using DG.Tweening;

namespace BounceDudes
{
    /// <summary>
    /// Class that manage usefull information about level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        static protected LevelManager _instance = null;
        static public LevelManager Instance { get { return LevelManager._instance; } }

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
        
        public void Awake()
        {
            LevelManager._instance = this;
            this._baseHpBakp = this._playerBase.HP;
            GameManager.Instance.OnStateChange += this.StateChangeCallback;
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
                GameManager.Instance.AddLevelInfo(GameManager.Instance.CurrentLevel.Id, info);
            
            if (win)
            {
                this._winPanel.Show();
            }
            else
            {
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
                this._pausePanel.Show();
            }
            else
            {
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
            color.a = 1;
            image.DOBlendableColor(color, .5f).OnComplete(() =>
            {
                if (callback != null)
                    callback();
            });
        }

        public void Quit()
        {
            GameManager.Instance.LoadScene("MapMenu");
            /*
            this.UnpauseGame();
            this.GameOver();
             */
        }

        public void PlayAgain()
        {
            GameManager.Instance.LoadScene(GameManager.Instance.LastLevel.SceneName);
            //SceneManager.LoadScene(GameManager.Instance.LastLevel.SceneName);
        }

        public void Dispose()
        {
            GameManager.Instance.OnStateChange -= this.StateChangeCallback;
        }
    }
}