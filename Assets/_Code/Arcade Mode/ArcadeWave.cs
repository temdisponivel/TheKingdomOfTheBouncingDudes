using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BounceDudes{

	public class ArcadeWave : MonoBehaviour {
		
		public List<List<Wave>> _arcadeWaves;
		protected int _arcadeWavesIndex = 0;

		public List<Wave> NextWave { get{ return _arcadeWaves[_arcadeWavesIndex];} }

		public void AddNewWave(List<Wave> newWaveList)
		{
			_arcadeWaves.Add (newWaveList);
		}


	}
}