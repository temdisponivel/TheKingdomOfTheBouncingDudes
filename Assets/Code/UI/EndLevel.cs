using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the end level scene.
    /// </summary>
    public class EndLevel : MonoBehaviour
    {
        public Transform _positionToCreateSoldierEarned = null;
        public string _messageWin = "";
        public string _messageLose = "";
        public Text _scoreText = null;
        public Text _enemiesKilled = null;
        public Text _messageText = null;

        public void Start()
        {
            LevelInformation info = GameManager.Instance.LevelsInformation[GameManager.Instance.LastLevel];
            if (info.EarnSoldier)
            {
                GameObject.Instantiate(info.Soldier, this._positionToCreateSoldierEarned.position, this._positionToCreateSoldierEarned.rotation);
            }
            this._scoreText.text = info.Score.ToString(".2");
            this._enemiesKilled.text = info.EnemiesKilled.ToString();
            this._messageText.text = info.Finished ? this._messageWin : this._messageLose;
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene(GameManager.Instance.LastLevel);
        }

        public void Continue()
        {
            SceneManager.LoadScene("LevelChooser");
        }
    }
}