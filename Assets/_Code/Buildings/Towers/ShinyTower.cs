﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a shiny tower.
    /// </summary>
    public class ShinyTower : Tower
    {
        override public void AffectObject(GameObject affected)
        {
			Character character = affected.GetComponent<Character> ();
			if(!character.IsShinyAttached){
				EffectManager.Instance.AttachShinyEffect (affected.transform);
				character.IsShinyAttached = true;
			}
        }
    }
}
