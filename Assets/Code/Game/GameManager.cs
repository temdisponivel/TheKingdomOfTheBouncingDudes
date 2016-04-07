using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BounceDudes
{
    /// <summary>
    /// Singleton for managing the gameplay.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        static protected GameManager _instance = null;
        static public GameManager Instance { get { return GameManager._instance; } }

        public List<GameObject> _allSoldiers = null;
        public List<GameObject> _allMonsters = null;
        public List<int> _availableSoldiersId = null;
        protected List<GameObject> AvailableSoldiers = null;
        
        public Dictionary<string, LevelInformation> LevelsInformation { get; set; }

        public Dictionary<int, string> SoldierNames { get; set; }
        
        public void Start()
        {
            if (GameManager.Instance == null)
            {
                GameManager._instance = this;
                GameObject.DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
                return;
            }
            this.LevelsInformation = new Dictionary<string, LevelInformation>();
            this.SoldierNames = new Dictionary<int, string>();
        }

        public List<GameObject> GetAvailableSoldiers()
        {
            this.AvailableSoldiers = new List<GameObject>();
            foreach (var soldier in this._allSoldiers)
            {

                if (this._availableSoldiersId.Contains(soldier.GetComponent<Character>()._id))
                {
                    this.AvailableSoldiers.Add(soldier);
                }
            }
            return this.AvailableSoldiers;
        }

        public List<GameObject> GetAllMonsters()
        {
            return this._allMonsters;
        }

        public void GameOver()
        {
            Debug.Log("GAME OVER");
        }
    }
}