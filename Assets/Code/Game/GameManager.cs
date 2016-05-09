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
        public List<GameObject> _allSoldiersRepresentation = null;
        public List<GameObject> _allMonsters = null;
        public List<int> _availableSoldiersId = null;
        protected List<GameObject> AvailableSoldiers = null;
        
        public Dictionary<string, LevelInformation> LevelsInformation { get; set; }

        public Dictionary<int, string> SoldierNames { get; set; }

        public string LastLevel { get; set; }

        public Dictionary<int, GameObject> Soldiers { get; set; }
        public Dictionary<int, GameObject> Monsters { get; set; }
        
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
            this.Soldiers = new Dictionary<int, GameObject>();
            this.Monsters = new Dictionary<int, GameObject>();

            foreach (var soldier in this._allSoldiers)
            {
                this.Soldiers[soldier.GetComponent<Character>()._id] = soldier;
            }

            foreach (var monster in this._allMonsters)
            {
                this.Monsters[monster.GetComponent<Character>()._id] = monster;
            }
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

        public GameObject GetRepresentationOfSoldier(int id)
        {
            foreach (var soldier in _allSoldiersRepresentation)
            {
                if (soldier.GetComponent<Character>()._id == id)
                {
                    return soldier;
                }
            }
            return null;
        }

        public List<GameObject> GetAllMonsters()
        {
            return this._allMonsters;
        }

        public void GameOver()
        {
            Debug.Log("GAME OVER");
        }

        public void AddLevelInfo(string id, LevelInformation info)
        {
            if (this.LevelsInformation.ContainsKey(id))
            {
                if (!GameManager.Instance.LevelsInformation[id].Finished || GameManager.Instance.LevelsInformation[id].Score < info.Score)
                {
                    GameManager.Instance.LevelsInformation.Remove(id);
                    GameManager.Instance.LevelsInformation.Add(id, info);
                }
            }
            else
            {
                this.LevelsInformation.Add(id, info);
            }
            if (info.EarnSoldier)
            {
                if (!this._availableSoldiersId.Contains(info.SoldierId))
                {
                    this._availableSoldiersId.Add(info.SoldierId);
                }
            }
        }
    }
}
