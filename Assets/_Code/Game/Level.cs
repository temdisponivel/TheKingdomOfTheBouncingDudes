using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using BounceDudes;
using UnityEngine;

namespace BounceDudes
{
    [Serializable]
    public class Level
    {
        public LevelId Id;
        public Sprite LevelImage;
        public string SceneName;
        public string Name;

		[TextArea(3, 10)]
        public string Description;

        public int[] StarByScore;
        public SoldierByChallengeDictionaryHack SoldiersByChallengeHackOne;
        public SoldierByChallengeDictionaryHack SoldiersByChallengeHackTwo;
        public SoldierByChallengeDictionaryHack SoldiersByChallengeHackThree;
    }
}
