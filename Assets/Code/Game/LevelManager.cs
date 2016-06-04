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

        [Header("Give Away")]
        public int[] StarByScore;

        public SoldierByChallengeDicionaryHack[] SoldiersByChallengeHack;

        public int EnemiesKilled { get; set; }

        protected Dictionary<Challenge, int[]> SoldierByChallenge;

        public void Start()
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
            List<int> soldiersEarned = new List<int>();
            foreach (var challenge in this.SoldierByChallenge)
            {
                if (ChallengeManager.ValidateCompletion(challenge.Key))
                {
                    soldiersEarned.AddRange(challenge.Value);
                }
            }
            info.SoldiersEarned = soldiersEarned.Distinct().ToArray();
            GameManager.Instance.AddLevelInfo(this.CurrentLevel, info);
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
    }
}