using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Singleton for managing the gameplay.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        static protected GameManager _instance = null;
        static public GameManager Instance { get { return GameManager._instance; } }
        
        public void Start()
        {
            if (GameManager.Instance == null)
            {
                GameManager._instance = this;
            }
        }
    }
}