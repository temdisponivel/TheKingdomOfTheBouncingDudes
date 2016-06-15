using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    public class HPTimer : MonoBehaviour
    {
        public float MaxSizeX = 0;
        public float MaxHP = 0;

		protected Vector2 _delta;

        public void Start()
        {
			_delta = this.GetComponent<RectTransform> ().sizeDelta;
			//this.GetComponent<RectTransform>().sizeDelta = new Vector2()
			//this.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._size + .15f, y = delta.y };
			this.MaxSizeX = _delta.x;
            this.MaxHP = LevelManager.Instance._playerBase.HP;
        }

        public void LateUpdate()
        {
			var deltaAux = new Vector2
			{ 
				x = MaxSizeX * (LevelManager.Instance._playerBase.HP / this.MaxHP), 
				y = _delta.y
			};


			if (deltaAux.x <= MaxSizeX)
				this.GetComponent<RectTransform> ().sizeDelta = deltaAux;
        }
    }
}
