using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Assets.Code.Game;
using Assets._Code.Game;
using DG.Tweening;
using DigitalRuby.SoundManagerNamespace;
using UnityEngine.SceneManagement;
using UnityEngineInternal;


namespace BounceDudes
{
    /// <summary>
    /// Singleton for managing the gameplay.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        static protected GameManager _instance = null;
        static public GameManager Instance { get { return GameManager._instance; } }

		static protected GooglePlayManager _googlePlayManagerInstance = new GooglePlayManager();
		static public GooglePlayManager GPManagerInstance { get { return GameManager._googlePlayManagerInstance; } }

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

        public List<int> FirstSoldiersToGive = new List<int>();

        public List<GameObject> _specialProjectiles = null;

        public List<GameObject> _allSoldiers = null;
		public List<GameObject> _allSoldiersRepresentation = null;

        public List<GameObject> _allMonsters = null;

        public Dictionary<int, List<int>> _availableSoldierInstanceIdById = new Dictionary<int, List<int>>();

        public Dictionary<LevelId, LevelInformation> LevelsInformation { get; set; }

        public Dictionary<int, List<string>> SoldierNames { get; set; }

        public Dictionary<int, GameObject> Monsters { get; set; }

        public Dictionary<int, GameObject> Soldiers { get; set; }

        public Level LastLevel { get; set; }

        public List<KeyValuePair<int, string>> NextLevelSoldiersDefinition = new List<KeyValuePair<int, string>>();

        public List<GameObject> NextLevelSoldiers
        {
            get
            {
                var result = new List<GameObject>();

                foreach (var soldier in NextLevelSoldiersDefinition)
                {
                    GameObject gameObject = (GameObject)Instantiate(Soldiers[soldier.Key]);
                    gameObject.GetComponent<Soldier>()._soldierName = soldier.Value;
                    result.Add(gameObject);
                }

                return result;
            }
        }

        public List<Level> Levels = new List<Level>();

        public Level CurrentLevel { get; set; }

        public Dictionary<LevelId, Level> LevelsById = new Dictionary<LevelId, Level>();

        public List<float> StarsPercent = new List<float>();

        public List<int> ChallengesComplete
        {
            get
            {
                var result = new List<int>();
                var levelsInfo = LevelsInformation.ToList();
                for (int i = 0; i < levelsInfo.Count; i++)
                {
                    if (levelsInfo[i].Value.Finished)
                    {
                        result.AddRange(levelsInfo[i].Value.ChallengesCompleted.Keys.Select(k => k.Id));
                    }
                }
                return result;
            }
        }
        public List<AchivmentId> UnlockedAchivments = new List<AchivmentId>();

        public List<Achivment> Achivments = new List<Achivment>();

        public int MaxSoldierInLevel = 6;

        public List<string> LooseGameMessages = new List<string>();

        public DayTimeSequence CurrentDayTimeSequence = DayTimeSequence.Morning;

		public List<SoldierClass> AllSoldierClasses = new List<SoldierClass>();

        [NonSerialized]
        public bool PassSplashScreen = false;
		public float AspectRatio = 10f / 16f;
		public bool FromMainMenuToMapMenu = true;

		public bool SoundVolume { get; set; }
		public bool MusicVolume { get; set; }


        public void Awake()
        {

			Camera.main.aspect = AspectRatio;


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

            this.Soldiers = new Dictionary<int, GameObject>();

            foreach (var soldier in _allSoldiers)
            {
                this.Soldiers[soldier.GetComponent<Character>()._id] = soldier;
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


            this.LoadGame();

            if (this._availableSoldierInstanceIdById.Count == 0)
            {
                for (int i = 0; i < FirstSoldiersToGive.Count; i++)
                {
                    this._availableSoldierInstanceIdById[FirstSoldiersToGive[i]] = new List<int>() { 0 };
                    this.AddNameToSoldier(Soldiers[FirstSoldiersToGive[i]].GetComponent<Soldier>()._soldierName, FirstSoldiersToGive[i], 0);
                }
                this.SaveGame();
            }

            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
			

        }

        void Start()
        {
            this.SoundVolume = SoundManager.SoundVolume != 0;
            this.MusicVolume = SoundManager.MusicVolume != 0;
        }

        public List<GameObject> GetAvailableSoldiers()
        {
            var _availableSoldiers = new List<GameObject>();
            foreach (var soldier in this._allSoldiers)
            {
                if (this._availableSoldierInstanceIdById.ContainsKey(soldier.GetComponent<Character>()._id))
                {
                    var soldierScript = soldier.GetComponent<Soldier>();
                    int index = 0;

                    foreach (var soldiers in this._availableSoldierInstanceIdById[soldierScript._id])
                    {
						if (soldiers != 0) {} // To supress warning, Blame Matheus

                        var soldierCopy = Instantiate(soldier);
                        soldierCopy.GetComponent<Soldier>()._soldierName = SoldierNames[soldierScript._id][index++];
                        _availableSoldiers.Add(soldierCopy);


                    }
                }
            }
            return _availableSoldiers;
        }

        public List<GameObject> GetAvailableSoldiersRepresentation()
        {
            var _availableSoldiers = new List<GameObject>();

            foreach (var soldier in this._allSoldiers)
            {
                if (this._availableSoldierInstanceIdById.ContainsKey(soldier.GetComponent<Character>()._id))
                {
                    var soldierScript = soldier.GetComponent<Soldier>();

                    int index = 0;

                    foreach (var soldiers in this._availableSoldierInstanceIdById[soldierScript._id])
                    {
						if (soldiers != 0) {} // To supress warning, Blame Matheus

                        var soldierCopy = Instantiate(GetRepresentationOfSoldier(soldierScript._id));

                        var soldierCopyScript = soldierCopy.GetComponent<Soldier>();

                        soldierCopyScript._soldierName = SoldierNames[soldierScript._id][index++];

                        soldierCopyScript._statSpeed = soldierScript._statSpeed;
                        soldierCopyScript._hp = soldierScript._hp;
                        soldierCopyScript._size = soldierScript._size;
						soldierCopyScript._soldierClass = soldierScript._soldierClass;
						soldierCopyScript._soldierDescription = soldierScript._soldierDescription;

                        _availableSoldiers.Add(soldierCopy);


                    }
                }
            }

            return _availableSoldiers;
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
        }

        public void AddLevelInfo(LevelId id, LevelInformation info)
        {
            if (!info.Finished)
                return;

            var challengesComplete = this.ChallengesComplete;

            if (this.LevelsInformation.ContainsKey(id))
            {
                var currencLevelInfo = this.LevelsInformation[id];

                info.Star = Mathf.Max(info.Star, currencLevelInfo.Star);
                info.EnemiesKilled = Mathf.Max(info.EnemiesKilled, currencLevelInfo.EnemiesKilled);
                info.ShootCount = Mathf.Max(info.ShootCount, currencLevelInfo.ShootCount);

                var currentChallengs = currencLevelInfo.ChallengesCompleted.ToList();
                var newChallengs = info.ChallengesCompleted.ToList();

                var keys = newChallengs.Select(e => e.Key.Id);
                for (int i = 0; i < currentChallengs.Count; i++)
                {
                    if (!keys.Contains(currentChallengs[i].Key.Id))
                    {
                        newChallengs.Add(currentChallengs[i]);
                    }
                }

                info.ChallengesCompleted.Clear();
                foreach (var challeng in newChallengs)
                {
                    info.ChallengesCompleted.Add(challeng.Key, challeng.Value);
                }

                GameManager.Instance.LevelsInformation.Remove(id);
                GameManager.Instance.LevelsInformation.Add(id, info);
            }
            else
            {
                this.LevelsInformation.Add(id, info);
            }

            foreach (var challeng in info.ChallengesCompleted)
            {
                if (challengesComplete.Contains(challeng.Key.Id))
                    continue;

                foreach (var soldierId in challeng.Value)
                {
                    if (this._availableSoldierInstanceIdById.ContainsKey(soldierId))
                    {
                        this._availableSoldierInstanceIdById[soldierId].Add(this._availableSoldierInstanceIdById[soldierId].Count);
                    }
                    else
                    {
                        this._availableSoldierInstanceIdById[soldierId] = new List<int>() { 0 };
                    }

                    this.AddNameToSoldier(_allSoldiers.FirstOrDefault(g => g.GetComponent<Soldier>()._id == soldierId).GetComponent<Soldier>()._soldierName, soldierId, this._availableSoldierInstanceIdById[soldierId].Count - 1);
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
					Debug.Log (ex);
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
            this.CurrentDayTimeSequence = gameInfo.CurrentDayTimeSequence;
        }

        public GameInfomation UpdateToGameInfo()
        {
            return new GameInfomation()
            {
                Levels = this.LevelsInformation,
                AvailableSoldierInstanceIdById = this._availableSoldierInstanceIdById,
                SoldierNames = this.SoldierNames,
                UnleckedAchivments = UnlockedAchivments,
                CurrentDayTimeSequence = CurrentDayTimeSequence,
            };
        }

        public void AddNameToSoldier(string name, int soldierId, int instanceId)
        {
            if (this.SoldierNames.ContainsKey(soldierId))
            {
                if (this.SoldierNames[soldierId].Count > instanceId)
                {
                    this.SoldierNames[soldierId][instanceId] = name;
                }
                else
                {
                    this.SoldierNames[soldierId].Add(name);
                }
            }
            else
            {
                this.SoldierNames[soldierId] = new List<string>();
                this.SoldierNames[soldierId].Add(name);
            }
        }

        public void LoadScene(string sceneName)
        {
            CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(sceneName));
        }
			
		public void CreateFullSave(bool overrideNormalSave)
        {
            for (int i = 0; i < _allSoldiers.Count; i++)
            {
                this._availableSoldierInstanceIdById[_allSoldiers[i].GetComponent<Soldier>()._id] = new List<int>() { 0 };
                this.AddNameToSoldier(Soldiers[_allSoldiers[i].GetComponent<Soldier>()._id].GetComponent<Soldier>()._soldierName, _allSoldiers[i].GetComponent<Soldier>()._id, 0);
            }
            foreach (Level level in Levels)
            {
                this.AddLevelInfo(level.Id, new LevelInformation()
                {
                    ChallengesCompleted = new Dictionary<Challenge, int[]>(),
                    EnemiesKilled = 10,
                    Finished = true,
                    LevelId = level.Id,
                    ShootCount = 10,
                    Star = 1,
                });
            }
            var bkp = this.SaveFileName;
			this.SaveFileName = overrideNormalSave == true ? bkp : bkp + "full.save";
            this.SaveGame();
        }


    }
}
