using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BounceDudes
{
    public class Credits : MonoBehaviour
    {

		bool _pressedEscape = false;

		void Start(){
			#if UNITY_ANDROID
			GameManager.GPManagerInstance.UnlockAchievement (GPGSIds.achievement_who_made_this_piece_of_game);
			#endif
		}

		void Update(){
			if (Input.GetKeyDown(KeyCode.Escape) && !_pressedEscape)
			{
				_pressedEscape = true;
				this.Return ();
			}
		}

		public void FacebookButton(){
			//Application.OpenURL ("http://m.facebook.com/KingdomBounceGame");
		}
       
        public void Return()
        {
			AudioManager.Instance.PlayInterfaceSound (0);
            GameManager.Instance.LoadScene("TitleScreen");
        }
    }
}
