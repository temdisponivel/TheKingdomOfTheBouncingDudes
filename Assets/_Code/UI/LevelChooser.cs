using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the level chooser scene.
    /// </summary>
    public class LevelChooser : MonoBehaviour
    {
        public GameObject[] _levelsInfo;

        public void Start()
        {
            foreach (var levelInfo in this._levelsInfo)
            {
                Text text = levelInfo.GetComponentInChildren<Text>();
                if (!GameManager.Instance.LevelsInformation.ContainsKey(levelInfo.name))
                {
                    text.text = levelInfo.name;
                    continue;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(levelInfo.name);
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine();
                builder.Append("Finished: ");
                builder.AppendLine(GameManager.Instance.LevelsInformation[levelInfo.name].Finished ? "Yes" : "No");
                builder.Append("Score: ");
                builder.AppendLine(GameManager.Instance.LevelsInformation[levelInfo.name].Score.ToString(".2"));
                builder.Append("Shoots: ");
                builder.Append(GameManager.Instance.LevelsInformation[levelInfo.name].ShootCount);
                builder.Append("Enemies Kill: ");
                builder.Append(GameManager.Instance.LevelsInformation[levelInfo.name].EnemiesKilled);
                text.text = builder.ToString();
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}