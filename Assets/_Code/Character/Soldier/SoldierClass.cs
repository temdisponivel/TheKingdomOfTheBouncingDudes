using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using BounceDudes;
using UnityEngine;

namespace BounceDudes
{
	public enum SoldierClassEnum
	{
		BERSERK = 0,
		PRECISION = 1,
		RESEARCH = 2,
	}

	[Serializable]
	public class SoldierClass
	{

		public string _className;

		[TextArea(2, 10)]
		public string _classDescription;


	}
		

}
