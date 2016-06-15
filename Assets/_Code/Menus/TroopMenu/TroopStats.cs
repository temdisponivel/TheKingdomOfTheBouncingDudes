using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BounceDudes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

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

        public IEnumerator Start()
        {
            TroopStats.Instance = this;

            HorizontalScrollSnap.Instance.OnChangeSoldier += (soldier =>
            {
                this.UpdateInfo(soldier);
            });

            yield return new WaitForEndOfFrame();

            CurrentSoldier = SoldierArray.Instance.First;
            this.UpdateInfo(CurrentSoldier);
        }

        public void UpdateInfo(Soldier soldier)
        {
            this.CurrentSoldier = soldier;
            this.NameText.text = this.CurrentSoldier._soldierName;
            var delta = this.SizeImage.GetComponent<RectTransform>().sizeDelta;
            this.SizeImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._size + .15f, y = delta.y };
            delta = this.SpeedImage.GetComponent<RectTransform>().sizeDelta;
            this.SpeedImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._statSpeed + .15f, y = delta.y };
            delta = this.ArmorImage.GetComponent<RectTransform>().sizeDelta;
            this.ArmorImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._hp + .15f, y = delta.y };
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
            GameManager.Instance.AddNameToSoldier(this.NameText.text, CurrentSoldier._id, SoldierArray.Instance.GetInstanceId(CurrentSoldier));
            CurrentSoldier._soldierName = this.InputNameText.text;
        }

        public void Return()
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.LoadScene("TitleScreen");
            //SceneManager.LoadScene("TitleScreen");
        }
    }
}
