using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Just a clever hack to expose a dictionary-like structure in the inspector.
    /// </summary>
    [Serializable]
    public class Challenge
    {
        [Tooltip("Type of the challenge")]
        public ChallengeType _type;

        [Tooltip("_x to validate the challenge. For example: Kill _x enemies, where _x is the number of enemies")]
        public int _x;
    }
}
