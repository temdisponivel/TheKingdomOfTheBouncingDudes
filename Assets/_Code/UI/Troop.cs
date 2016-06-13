using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the troop scene.
    /// </summary>
    public class Troop : MonoBehaviour
    {
        [Serializable]
        public class SoldierInfo
        {
            public int Id;
            public string SoldierName;
            public int Velocity;
            public int HP;
            public GameObject SoldierRepresentation;
        }

        public SoldierInfo[] _soldiersInfo = null;

        public void Start()
        {
            this.UpdateSoldiersInfo();
        }

        public void EndEditName(int id, string newName)
        {
            if (GameManager.Instance.SoldierNames.ContainsKey(id))
            {
                GameManager.Instance.SoldierNames.Remove(id);
            }
            else
                GameManager.Instance.SoldierNames[id] = new List<string>();
            GameManager.Instance.SoldierNames[id].Add(newName);
            this.UpdateSoldiersInfo();
        }

        public void Return()
        {
            GameManager.Instance.SaveGame();
            SceneManager.LoadScene("LevelChooser");
        }

        protected void UpdateSoldiersInfo()
        {
            
        }
    }
}