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

        public SoldiersDictionaryHack[] SoldiersToGiveByStar;

        public int EnemiesKilled { get; set; }

        protected Dictionary<int, int[]> SoldierByStar;

        public void Start()
        {
            LevelManager._instance = this;
            this.CurrentLevel = SceneManager.GetActiveScene().name;
            GameManager.Instance.LastLevel = this.CurrentLevel;
            this.Score = 0;
            this.SoldierByStar = new Dictionary<int, int[]>();
            foreach (var soldierByStar in this.SoldiersToGiveByStar)
            {
                this.SoldierByStar[soldierByStar.Star] = soldierByStar.SoldierToGive;
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
            info.SoldiersEarned = new int[this.SoldierByStar[info.Star].Length];
            if (this.SoldierByStar.ContainsKey(info.Star))
            {
                for (int i = 0, soldierId = 0; i < this.SoldierByStar[info.Star].Length; soldierId = this.SoldierByStar[info.Star][i++])
                {
                    info.SoldiersEarned[i] = soldierId;
                }   
            }
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