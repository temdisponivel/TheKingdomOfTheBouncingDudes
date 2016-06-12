using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Game;

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

        public string CurrentLevel { get; set; }
        public float Score { get; set; }

        public Text _soldierNameText;

        public GameObject _pausePanel = null;

        [Header("Give Away")]
        public int[] StarByScore;

        public SoldierByChallengeDictionaryHack[] SoldiersByChallengeHack;

        public int EnemiesKilled { get; set; }

        protected Dictionary<Challenge, int[]> SoldierByChallenge;

        public void Awake()
        {
            LevelManager._instance = this;
            this.CurrentLevel = SceneManager.GetActiveScene().name;
            GameManager.Instance.LastLevel = this.CurrentLevel;
            this.Score = 0;
            this.SoldierByChallenge = new Dictionary<Challenge, int[]>();
            foreach (var soldierByStar in this.SoldiersByChallengeHack)
            {
                this.SoldierByChallenge[soldierByStar._challenge] = soldierByStar._soldierToGive;
            }

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
            this.CalculateScore();
            LevelInformation info = new LevelInformation();
            info.Score = this.Score;
            info.EnemiesKilled = this.EnemiesKilled;
            info.Finished = win;
            info.ShootCount = Weapon.Instance.ShootCount;
            info.Star = this.StarByScore.Where(s => s >= this.Score).Count();
            Dictionary<Challenge, int[]> soldiersEarned = new Dictionary<Challenge, int[]>();
            foreach (var challenge in this.SoldierByChallenge)
            {
                if (ChallengeManager.ValidateCompletion(challenge.Key))
                {
                    soldiersEarned.Add(challenge.Key, challenge.Value);
                }
            }
            info.ChallengesCompleted = soldiersEarned;
            GameManager.Instance.AddLevelInfo(this.CurrentLevel, info);
            GameManager.Instance.OnStateChange -= this.StateChangeCallback;
            SceneManager.LoadScene("EndLevel");
        }

        public void KillEnemy(Character enemy)
        {
            this.Score += (enemy._pointsWhenKilled * ComboManager.Instance.CurrentPointsMultiplier);
            this.EnemiesKilled++;
        }

        public void CalculateScore()
        {
            this.Score += this._playerBase.HP + this._enemyBase.HP;
            this.Score = Mathf.Max(0, this.Score);
        }

        public void StateChangeCallback()
        {
            if (GameManager.Instance.State == GameState.PAUSED)
            {
                this._pausePanel.SetActive(true);
            }
            else
            {
                this._pausePanel.SetActive(false);
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

        public void Quit()
        {
            this.UnpauseGame();
            this.GameOver();
        }
    }
}