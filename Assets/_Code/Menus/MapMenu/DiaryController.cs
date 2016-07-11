using UnityEngine;
using System.Collections;
using Assets._Code.Game;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.UI;


namespace BounceDudes
{

    /// <summary>
    /// Class that controls the Diary in the Map Menu.
    /// </summary>
    /// 
    public class DiaryController : MonoBehaviour
    {

		public GameObject _shade;

        public Image _levelImage;
        public Text _levelName;
        public Text _levelDesc;

        public Image _starOne;
        public Image _starTwo;
        public Image _starThree;

        public Image _challengeImageOne;
        public Image _challengeImageTwo;
        public Image _challengeImageThree;

        public Text _challengeDescOne;
        public Text _challengeDescTwo;
        public Text _challengeDescThree;

        public static WinLevelPanel Instance;

        public Image ChallengeOneSoldierIcon;
        public Image ChallengeTwoSoldierIcon;
        public Image ChallengeThreeSoldierIcon;

        public Image ChallengeOneCheck;
        public Image ChallengeTwoCheck;
        public Image ChallengeThreeCheck;
        
		public Image ChallengeOneLock;
		public Image ChallengeTwoLock;
		public Image ChallengeThreeLock;

        public void Show()
        {
			this._shade.transform.DOScale(1, 0.2f);
			this.transform.DORotate (new Vector3 (0, 0, 0), 0.4f, RotateMode.Fast).OnComplete (this.toggleShadeComplete);
            //this.transform.DOMove(this._shownPosition.transform.position, 0.5f);
        }

        public void Hide()
        {
			this._shade.transform.DOScale(0, 0.2f);
			this.transform.DORotate (new Vector3 (0, 0, -90), 0.4f, RotateMode.Fast).OnComplete (this.toggleShadeComplete);
            MapMenu.Instance._lastNode = null;
            //this.transform.DOMove(this._hidenPosition.transform.position, 0.5f);
        }

		// DOTween intelisense <Complete>
		protected void toggleShadeComplete(){
			Color fadeColor = new Color (1,1,1,255/140);
			DOTween.To (() => fadeColor, x => fadeColor = x, fadeColor, 1).SetOptions (true);
		}

        public void UpdateInfo(LevelId levelId)
        {
            Level level;

            if (GameManager.Instance.LevelsById.ContainsKey(levelId))
                level = GameManager.Instance.LevelsById[levelId];
            else
                level = new Level();

            LevelInformation levelInfo;

            if (GameManager.Instance.LevelsInformation.ContainsKey(levelId))
                levelInfo = GameManager.Instance.LevelsInformation[levelId];
            else
                levelInfo = new LevelInformation();

            this._levelImage.sprite = level.LevelImage;
            this._levelName.text = level.Name;
            this._levelDesc.text = level.Description;

			this._levelName.GetComponent<TextToTraslate> ().Translate ();
			this._levelDesc.GetComponent<TextToTraslate> ().Translate ();

            switch (levelInfo.Star)
            {
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

			var challengesComplete = GameManager.Instance.ChallengesComplete;

			Challenge currentChallenge = level.SoldiersByChallengeHackOne._challenge;
			currentChallenge.SetDescription ();
            _challengeDescOne.text = currentChallenge.Description;
			_challengeDescOne.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);

            ChallengeOneSoldierIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackOne._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChallenge.Id))
            {
                _challengeImageOne.enabled = true;
                ChallengeOneCheck.enabled = true;
				ChallengeOneLock.enabled = false;
				ChallengeOneSoldierIcon.color = Color.white;
            }
            else
            {
                _challengeImageOne.enabled = false;
                ChallengeOneCheck.enabled = false;
				ChallengeOneLock.enabled = true;
				ChallengeOneSoldierIcon.color = Color.gray;
            }

            currentChallenge = level.SoldiersByChallengeHackTwo._challenge;
			currentChallenge.SetDescription ();
            _challengeDescTwo.text = currentChallenge.Description;
			_challengeDescTwo.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);

            ChallengeTwoSoldierIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackTwo._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChallenge.Id))
            {
                _challengeImageTwo.enabled = true;
                ChallengeTwoCheck.enabled = true;
				ChallengeTwoLock.enabled = false;
				ChallengeTwoSoldierIcon.color = Color.white;

            }
            else
            {
                _challengeImageTwo.enabled = false;
                ChallengeTwoCheck.enabled = false;
				ChallengeTwoLock.enabled = true;
				ChallengeTwoSoldierIcon.color = Color.gray;
            }

            currentChallenge = level.SoldiersByChallengeHackThree._challenge;
			currentChallenge.SetDescription ();
            _challengeDescThree.text = currentChallenge.Description;
			_challengeDescThree.GetComponent<TextToTraslate> ().TranslateWithOneArgument (currentChallenge._x);

            ChallengeThreeSoldierIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackThree._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChallenge.Id))
            {
                _challengeImageThree.enabled = true;
                ChallengeThreeCheck.enabled = true;
				ChallengeThreeLock.enabled = false;
				ChallengeThreeSoldierIcon.color = Color.white;
            }
            else
            {
                _challengeImageThree.enabled = false;
                ChallengeThreeCheck.enabled = false;
				ChallengeThreeLock.enabled = true;
				ChallengeThreeSoldierIcon.color = Color.gray;
            }
        }
    }

}
