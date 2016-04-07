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

        public int EnemiesKilled { get; set; }

        public void Start()
        {
            LevelManager._instance = this;
            this.CurrentLevel = SceneManager.GetActiveScene().name;
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
            GameManager.Instance.LevelsInformation.Add(this.CurrentLevel, info);
            SceneManager.LoadScene("LevelChooser");
        }

        public void KillEnemy(Character enemy)
        {
            this.Score += enemy._pointsWhenKilled;
            this.EnemiesKilled++;
        }

        public void CalculateScore()
        {
            this.Score += this._playerBase.HP + this._enemyBase.HP;
        }
    }
}