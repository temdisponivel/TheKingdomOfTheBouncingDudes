using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the menu for the title scene.
    /// </summary>
    public class Title : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("LevelChooser");
        }
    }
}