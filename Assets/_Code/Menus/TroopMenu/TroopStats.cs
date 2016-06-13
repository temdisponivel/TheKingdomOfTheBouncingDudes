using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BounceDudes;
using UnityEngine;
using UnityEngine.UI;

namespace BounceDudes
{
    public class TroopStats : MonoBehaviour
    {
        public static TroopStats Instance = null;

        public Text NameText;
        public InputField InputNameText;

        public Image SizeImage;
        public Image SpeedImage;
        public Image ArmorImage;

        public Soldier CurrentSoldier;

        public void Start()
        {
            TroopStats.Instance = this;


        }

        public void UpdateInfo(Soldier soldier)
        {
            this.CurrentSoldier = soldier;
        }

        public void OnClickTextName()
        {
            this.NameText.gameObject.SetActive(false);
            this.InputNameText.gameObject.SetActive(true);
            this.InputNameText.text = this.CurrentSoldier._soldierName;
            this.InputNameText.ActivateInputField();
        }

        public void OnEndEditName()
        {
            this.NameText.gameObject.SetActive(true);
            this.InputNameText.gameObject.SetActive(false);
            this.NameText.text = this.InputNameText.text;
        }
    }
}
