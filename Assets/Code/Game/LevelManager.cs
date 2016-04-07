using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        public float _pointsToGive = 10;
        public bool _giveSoldier = false;
        public int _soldierToGive = 0;

        public int EnemiesKilled { get; set; }

        public void Start()
        {
            LevelManager._instance = this;
            this.CurrentLevel = SceneManager.GetActiveScene().name;
            GameManager.Instance.LastLevel = this.CurrentLevel;
            this.Score = 0;
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
            if (this._giveSoldier && this.Score > this._pointsToGive)
            {
                info.EarnSoldier = true;
                info.SoldierId = this._soldierToGive;
            }
            else
            {
                info.EarnSoldier = false;
            }
            GameManager.Instance.AddLevelInfo(this.CurrentLevel, info);
            SceneManager.LoadScene("EndLevel");
        }

        public void KillEnemy(Character enemy)
        {
            this.Score += enemy._pointsWhenKilled;
            this.EnemiesKilled++;
        }

        public void CalculateScore()
        {
            this.Score += this._playerBase.HP + this._enemyBase.HP;
            this.Score = Mathf.Max(0, this.Score);
        }
    }
}