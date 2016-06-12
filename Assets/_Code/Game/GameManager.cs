﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Assets.Code.Game;
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
                if (this.OnStateChange  != null)
                    this.OnStateChange();
            }
        }

        public string SaveFileName = "boucedudes.save";

        public string SaveFilePath { get { return String.Format("{0}{1}{2}", Application.persistentDataPath, "/", SaveFileName); } }

        public List<GameObject> _specialProjectiles = null;

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

        public List<GameObject> NextLevelSoldiers = new List<GameObject>();

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

            this.LevelsInformation = new Dictionary<string, LevelInformation>();;
            this.SoldierNames = new Dictionary<int, string>();
            this.Soldiers = new Dictionary<int, GameObject>();
            this.Monsters = new Dictionary<int, GameObject>();

            this.LoadGame();

            foreach (var soldier in this._allSoldiers)
            {
                this.Soldiers[soldier.GetComponent<Character>()._id] = soldier;
            }

            foreach (var monster in this._allMonsters)
            {
                this.Monsters[monster.GetComponent<Character>()._id] = monster;
            }

            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);


			//NextLevelSoldiers = this.GetAvailableSoldiers(); // FOR TESTS ONLY
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
            this.SaveGame();
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
            foreach (var challeng in info.ChallengesCompleted)
            {
                foreach (var soldierId in challeng.Value)
                {
                    if (!this._availableSoldiersId.Contains(soldierId))
                    {
                        this._availableSoldiersId.Add(soldierId);
                    }
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
                catch(SerializationException ex)
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
            this._availableSoldiersId = gameInfo.Soldiers.ToList();
            this.SoldierNames = gameInfo.SoldierNames;
        }

        public GameInfomation UpdateToGameInfo()
        {
            return new GameInfomation()
            {
                Levels = this.LevelsInformation,
                Soldiers = this._availableSoldiersId.ToArray(),
                SoldierNames = this.SoldierNames,
            };
        }

        
    }
}
