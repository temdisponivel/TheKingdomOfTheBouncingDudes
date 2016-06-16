using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using BounceDudes;
using UnityEngine;

namespace Assets.Code.Game
{
    /// <summary>
    /// Class that holds information to be saved/loaded.
    /// </summary>
    [Serializable]
    public class GameInfomation
    {
        public Dictionary<int, List<int>> AvailableSoldierInstanceIdById = new Dictionary<int, List<int>>();
        public Dictionary<LevelId, LevelInformation> Levels;
        public Dictionary<int, List<string>> SoldierNames;
        public List<AchivmentId> UnleckedAchivments;
        public DayTimeSequence CurrentDayTimeSequence;
    }
}
