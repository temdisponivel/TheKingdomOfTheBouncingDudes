using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;

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
            public string ClassName;
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
            GameManager.Instance.SoldierNames.Add(id, newName);
            this.UpdateSoldiersInfo();
        }

        public void Return()
        {
            SceneManager.LoadScene("LevelChooser");
        }

        protected void UpdateSoldiersInfo()
        {
            foreach (var soldierInfo in this._soldiersInfo)
            {
                if (!GameManager.Instance._availableSoldiersId.Contains(soldierInfo.Id))
                {
                    soldierInfo.SoldierRepresentation.SetActive(false);
                    continue;
                }
                Text text = soldierInfo.SoldierRepresentation.GetComponentInChildren<Text>();
                StringBuilder builder = new StringBuilder();
                if (GameManager.Instance.SoldierNames.ContainsKey(soldierInfo.Id))
                {
                    builder.AppendLine(GameManager.Instance.SoldierNames[soldierInfo.Id]);
                    builder.AppendLine();
                }
                builder.AppendLine();
                builder.Append(soldierInfo.ClassName);
                builder.AppendLine();
                builder.Append("HP: ");
                builder.Append(soldierInfo.HP);
                builder.AppendLine();
                builder.Append("Velocity: ");
                builder.Append(soldierInfo.Velocity);
                text.text = builder.ToString();
            }
        }
    }
}