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
        public Level[] _levelsInfo;

        public void Start()
        {
            
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}