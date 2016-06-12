using UnityEngine;
using System.Collections;
using System.Linq;
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
        public Text _starText = null;
        public Text _enemiesKilled = null;
        public Text _shootCountText = null;
        public Text _messageText = null;

        public void Start()
        {
            LevelInformation info = GameManager.Instance.LevelsInformation[GameManager.Instance.LastLevel];
            foreach (var challeng in info.ChallengesCompleted)
            {
                foreach (var soldierId in challeng.Value)
                {
                    GameObject.Instantiate(GameManager.Instance.GetRepresentationOfSoldier(soldierId),
                        this._positionToCreateSoldierEarned.position, this._positionToCreateSoldierEarned.rotation);
                }
            }
            this._scoreText.text = info.Score.ToString(".2");
            this._starText.text = info.Star.ToString();
            this._enemiesKilled.text = info.EnemiesKilled.ToString();
            this._shootCountText.text = info.ShootCount.ToString();
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
};