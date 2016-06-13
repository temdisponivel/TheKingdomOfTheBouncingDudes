using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Assets.Code.Game;
using Assets._Code.Game;
using DG.Tweening;

namespace BounceDudes
{
    /// <summary>
    /// Singleton for managing the gameplay.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        static protected GameManager _instance = null;
        static public GameManager Instance { get { return GameManager._instance; } }

        public event Action OnStateChange;

        private GameState _state;

        private float _timeScaleBkp;

        public GameState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state == GameState.PLAYING && value == GameState.PAUSED)
                {
                    this._timeScaleBkp = Time.timeScale;
                    Time.timeScale = 0.0001f;
                }
                else if (_state == GameState.PAUSED && value != GameState.PAUSED)
                {
                    Time.timeScale = this._timeScaleBkp;
                }
                _state = value;
                if (this.OnStateChange != null)
                    this.OnStateChange();
            }
        }

        public string SaveFileName = "boucedudes.save";

        public string SaveFilePath { get { return String.Format("{0}{1}{2}", Application.persistentDataPath, "/", SaveFileName); } }

        public List<GameObject> _specialProjectiles = null;

        public List<GameObject> _allSoldiers = null;
        public List<GameObject> _allSoldiersRepresentation = null;

        public List<GameObject> _allMonsters = null;

        protected List<GameObject> AvailableSoldiers = null;

        public Dictionary<int, List<int>> _availableSoldierInstanceIdById = new Dictionary<int, List<int>>();

        public Dictionary<LevelId, LevelInformation> LevelsInformation { get; set; }

        public Dictionary<int, List<string>> SoldierNames { get; set; }

        public Dictionary<int, GameObject> Monsters { get; set; }

        public Level LastLevel { get; set; }

        public List<GameObject> NextLevelSoldiers = new List<GameObject>();

        public List<Level> Levels = new List<Level>();

        public Level CurrentLevel { get; set; }

        public Dictionary<LevelId, Level> LevelsById = new Dictionary<LevelId, Level>();

        public List<int> ChallengesComplete = new List<int>();
        public List<AchivmentId> UnlockedAchivments = new List<AchivmentId>();

        public List<Achivment> Achivments = new List<Achivment>();

        public int MusicVolume = 1;
        public int SoundVolume = 1;

        public void Awake()
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

            this.LevelsInformation = new Dictionary<LevelId, LevelInformation>(); ;
            this.SoldierNames = new Dictionary<int, List<string>>();

            this.LoadGame();

            if (this._availableSoldierInstanceIdById.Count == 0)
            {
                this._availableSoldierInstanceIdById[1] = new List<int>() {0};
                this.AddNameToSoldier(_allSoldiers[0].GetComponent<Soldier>()._soldierName, 1, 0);
                this.SaveGame();
            }

            this.Monsters = new Dictionary<int, GameObject>();

            foreach (var monster in _allMonsters)
            {
                this.Monsters[monster.GetComponent<Character>()._id] = monster;
            }

            foreach (var level in Levels)
            {
                LevelsById[level.Id] = level;
            }

            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        }

        public List<GameObject> GetAvailableSoldiers()
        {
            this.AvailableSoldiers = new List<GameObject>();
            foreach (var soldier in this._allSoldiers)
            {
                if (this._availableSoldierInstanceIdById.ContainsKey(soldier.GetComponent<Character>()._id))
                {
                    int index = 0;
                    foreach (var soldiers in this._availableSoldierInstanceIdById[soldier.GetComponent<Character>()._id])
                    {
                        soldier.GetComponent<Soldier>()._soldierName =
                            SoldierNames[soldier.GetComponent<Character>()._id][index++];
                        this.AvailableSoldiers.Add(soldier);
                    }
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
            this.SaveGame();
            Debug.Log("GAME OVER");
        }

        public void AddLevelInfo(LevelId id, LevelInformation info)
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
            foreach (var challeng in info.ChallengesCompleted)
            {
                foreach (var soldierId in challeng.Value)
                {
                    if (this._availableSoldierInstanceIdById.ContainsKey(soldierId))
                        this._availableSoldierInstanceIdById[soldierId].Add(this._availableSoldierInstanceIdById[soldierId].Count + 1);
                    else
                        this._availableSoldierInstanceIdById[soldierId] = new List<int>() { 0 };
                    this.AddNameToSoldier(_allSoldiers.FirstOrDefault(g => g.GetComponent<Soldier>()._id == soldierId).GetComponent<Soldier>()._soldierName, soldierId, this._availableSoldierInstanceIdById[soldierId].Count);
                }
            }
            this.SaveGame();
        }

        public void SaveGame()
        {
            FileUtil.WriteToBinaryFile(this.SaveFilePath, this.UpdateToGameInfo());
        }

        public void LoadGame()
        {
            if (File.Exists(this.SaveFilePath))
            {
                try
                {
                    GameInfomation gameInfo = FileUtil.ReadFromBinaryFile<GameInfomation>(this.SaveFilePath);
                    this.UpdateFromGameInfo(gameInfo);
                }
                catch (SerializationException ex)
                {
                    File.Delete(this.SaveFilePath);
                }
            }
            else
            {
                this.SaveGame();
            }
        }

        public void UpdateFromGameInfo(GameInfomation gameInfo)
        {
            this.LevelsInformation = gameInfo.Levels;
            this._availableSoldierInstanceIdById = gameInfo.AvailableSoldierInstanceIdById;
            this.SoldierNames = gameInfo.SoldierNames;
            this.UnlockedAchivments = gameInfo.UnleckedAchivments;
        }

        public GameInfomation UpdateToGameInfo()
        {
            return new GameInfomation()
            {
                Levels = this.LevelsInformation,
                AvailableSoldierInstanceIdById = this._availableSoldierInstanceIdById,
                SoldierNames = this.SoldierNames,
                UnleckedAchivments = UnlockedAchivments,
            };
        }

        public void AddNameToSoldier(string name, int soldierId, int instanceId)
        {
            if (this.SoldierNames.ContainsKey(soldierId))
            {
                this.SoldierNames[soldierId][instanceId] = name;
            }
            else
            {
                this.SoldierNames[soldierId] = new List<string>();
                this.SoldierNames[soldierId].Add(name);
            }
        }

        public void MuteSound()
        {
            this.SoundVolume = 0;
        }

        public void MuteMusic()
        {
            this.MusicVolume = 0;
        }
    }
}
