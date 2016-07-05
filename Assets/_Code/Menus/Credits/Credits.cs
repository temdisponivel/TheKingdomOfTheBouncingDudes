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
		public void FacebookButton(){
			Application.OpenURL ("http://m.facebook.com/KingdomBounceGame");
		}
       
        public void Return()
        {
			AudioManager.Instance.PlayInterfaceSound (0);
            GameManager.Instance.LoadScene("TitleScreen");
        }
    }
}
