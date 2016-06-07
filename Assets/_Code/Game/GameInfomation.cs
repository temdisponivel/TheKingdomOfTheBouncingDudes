using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public int[] Soldiers;
        public Dictionary<string, LevelInformation> Levels;
        public Dictionary<int, string> SoldierNames;
    }
}
