using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    [Serializable]
    public class SoldierByChallengeDictionaryHack
    {
        [Tooltip("Challenge to complete")]
        public Challenge _challenge;

        [Tooltip("Soldiers to give when this challenge is completed.")]
        public int[] _soldierToGive;
    }
}
