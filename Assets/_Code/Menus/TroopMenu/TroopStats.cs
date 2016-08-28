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

		public GameObject DescriptionPanel;
		public Image ManIcon;
		public Image TextIcon;

		public Text SoldierDescriptionText;
		public Text SoldierClassNameText;
		public Text SoldierClassDescriptionText;

        public Soldier CurrentSoldier;

		protected bool InfoShown = false;

		protected bool _pressedEscape = false;

        public IEnumerator Start()
        {
            TroopStats.Instance = this;


            HorizontalScrollSnap.Instance.OnChangeSoldier += (soldier =>
            {
				AudioManager.Instance.PlayInterfaceSound (0);
                this.UpdateInfo(soldier);
				
            });

            yield return new WaitForEndOfFrame();

            CurrentSoldier = SoldierArray.Instance.First;
            this.UpdateInfo(CurrentSoldier);
        }

		void Update(){
			
			if (Input.GetKeyDown (KeyCode.Escape) && !_pressedEscape) {
				_pressedEscape = true;
				this.Return ();
			}
		}

        public void UpdateInfo(Soldier soldier)
        {
			
            this.CurrentSoldier = soldier;
            this.NameText.text = this.CurrentSoldier._soldierName;

			this.SoldierDescriptionText.text = this.CurrentSoldier._soldierDescription;
			this.SoldierClassNameText.text = GameManager.Instance.AllSoldierClasses[(int)this.CurrentSoldier._soldierClass]._className;
			this.SoldierClassDescriptionText.text = GameManager.Instance.AllSoldierClasses[(int)this.CurrentSoldier._soldierClass]._classDescription;

			this.TranslateFields ();

            var delta = this.SizeImage.GetComponent<RectTransform>().sizeDelta;
			this.SizeImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._size * 67.25f, y = delta.y };
            delta = this.SpeedImage.GetComponent<RectTransform>().sizeDelta;
            this.SpeedImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._statSpeed * 68.5f, y = delta.y };
            delta = this.ArmorImage.GetComponent<RectTransform>().sizeDelta;
            this.ArmorImage.GetComponent<RectTransform>().sizeDelta = new Vector2() { x = this.CurrentSoldier._hp * 68.5f, y = delta.y };
        }

        public void OnClickTextName()
        {
			AudioManager.Instance.PlayInterfaceSound (0);

            this.NameText.gameObject.SetActive(false);
            this.InputNameText.gameObject.SetActive(true);
            this.InputNameText.text = this.CurrentSoldier._soldierName;
            this.InputNameText.ActivateInputField();
        }

        public void OnEndEditName()
        {
			AudioManager.Instance.PlayInterfaceSound (0);

            this.NameText.gameObject.SetActive(true);
            this.InputNameText.gameObject.SetActive(false);
            this.NameText.text = this.InputNameText.text;
            GameManager.Instance.AddNameToSoldier(this.NameText.text, CurrentSoldier._id, SoldierArray.Instance.GetInstanceId(CurrentSoldier));
            CurrentSoldier._soldierName = this.InputNameText.text;

			GameManager.GPManagerInstance.UnlockAchievement (GPGSIds.achievement_put_your_friends__cat_in_the_game);

			GameManager.Instance.SaveGame();
        }

        public void Return()
        {
            //GameManager.Instance.SaveGame();
			AudioManager.Instance.PlayInterfaceSound (0);
            GameManager.Instance.LoadScene("MapMenu"); 

        }

		public void ToggleInfo()
		{
			AudioManager.Instance.PlayInterfaceSound (0);

			InfoShown = !InfoShown;

			DescriptionPanel.gameObject.SetActive (InfoShown);

			this.TranslateFields ();

			ManIcon.enabled = InfoShown;
			TextIcon.enabled = !InfoShown;

		}

		public void TranslateFields(){
			if (!InfoShown)
				return;
			
			this.SoldierDescriptionText.GetComponent<TextToTraslate> ().Translate();
			this.SoldierClassNameText.GetComponent<TextToTraslate> ().Translate();
			this.SoldierClassDescriptionText.GetComponent<TextToTraslate> ().Translate();
		}
			
    }
}
