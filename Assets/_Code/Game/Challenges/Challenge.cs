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
        public int Id;

        [Tooltip("Type of the challenge")]
        public ChallengeType _type;

		protected string _description;
		public string Description { get{ return _description; } }

        [Tooltip("_x to validate the challenge. For example: Kill _x enemies, where _x is the number of enemies")]
        public int _x;

		public void SetDescription(){
			
			string aux = "ERROR";

			switch (_type) {
			case ChallengeType.KillXEnemy:
				aux = "LD_Challenge_DefeatEnemies";
				break;
			case ChallengeType.MakeAComboWithXHits:
				aux = "LD_Challenge_GetCombo";
				break;
			case ChallengeType.ShootMaxXSoldier:
				aux = "LD_Challenge_ShootXTimes";
				break;
			case ChallengeType.StayWithXHP:
				aux = "LD_Challenge_SurviveHP";
				break;

			default:
				break;
			}
				
			_description = aux;
		}
    }
}
