﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using UnityEngine;
using UnityEngine.UI;

namespace BounceDudes
{
    public class LevelNode : MonoBehaviour
    {
        public LevelId LevelId;

        public Image _levelImage;

        public Image _starBorder;
        public Image _starOne;
        public Image _starTwo;
        public Image _starThree;

		public bool _levelLocked = false;

        public void Start()
        {
            Level level;

            if (GameManager.Instance.LevelsById.ContainsKey(LevelId))
                level = GameManager.Instance.LevelsById[LevelId];
            else
                level = new Level();

            LevelInformation levelInfo;

            if (GameManager.Instance.LevelsInformation.ContainsKey(LevelId))
                levelInfo = GameManager.Instance.LevelsInformation[LevelId];
            else
                levelInfo = new LevelInformation();


            this._levelImage.sprite = level.LevelImage;

            switch (levelInfo.Star){
           	case 1:
                _starOne.enabled = true;
                _starTwo.enabled = false;
                _starThree.enabled = false;
                break;
            case 2:
                _starOne.enabled = true;
                _starTwo.enabled = true;
                _starThree.enabled = false;
                break;
            case 3:
                _starOne.enabled = true;
                _starTwo.enabled = true;
                _starThree.enabled = true;
                break;
			default:
                _starOne.enabled = false;
                _starTwo.enabled = false;
                _starThree.enabled = false;
                break;
            }
				

			int count = levelInfo.GetCountChallengesCompleted;
            switch (count)
            {
			case 1:
				this._starBorder.sprite = MapMenu.Instance.StarBorderBronze;
				break;
            case 2:
                this._starBorder.sprite = MapMenu.Instance.StarBorderSilver;
                break;
            case 3:
                this._starBorder.sprite = MapMenu.Instance.StarBorderGold;
                break;
			default:
				this._starBorder.sprite = MapMenu.Instance.StarBorderTransparent;
				break;
            }
				

        }

		public void setLocked(bool value){
			_levelLocked = value;
			this._starBorder.enabled = !_levelLocked;
		}

    }


}
