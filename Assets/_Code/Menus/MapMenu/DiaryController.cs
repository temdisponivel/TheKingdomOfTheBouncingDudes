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

        public GameObject _shownPosition;
        public GameObject _hidenPosition;
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

        public Image ChallengeOneIcon;
        public Image ChallengeTwoIcon;
        public Image ChallengeThreeIcon;

        public Image ChallengeOneIconChecker;
        public Image ChallengeTwoIconChecker;
        public Image ChallengeThreeIconChecker;
        
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

            Challenge currentChellenge = level.SoldiersByChallengeHackOne._challenge;
            _challengeDescOne.text = currentChellenge.Description;
            var challengesComplete = GameManager.Instance.ChallengesComplete;
            _challengeImageOne.enabled = true;
            ChallengeOneIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackOne._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChellenge.Id))
            {
                _challengeImageOne.enabled = true;
                ChallengeOneIconChecker.enabled = true;
            }
            else
            {
                _challengeImageOne.enabled = false;
                ChallengeOneIconChecker.enabled = false;
            }

            currentChellenge = level.SoldiersByChallengeHackTwo._challenge;
            _challengeDescTwo.text = currentChellenge.Description;
            ChallengeTwoIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackTwo._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChellenge.Id))
            {
                _challengeImageTwo.enabled = true;
                ChallengeTwoIconChecker.enabled = true;
            }
            else
            {
                _challengeImageTwo.enabled = false;
                ChallengeTwoIconChecker.enabled = false;
            }

            currentChellenge = level.SoldiersByChallengeHackThree._challenge;
            _challengeDescThree.text = currentChellenge.Description;
            ChallengeThreeIcon.sprite = GameManager.Instance.GetRepresentationOfSoldier(level.SoldiersByChallengeHackThree._soldierToGive[0]).GetComponent<Image>().sprite;
            if (challengesComplete.Contains(currentChellenge.Id))
            {
                _challengeImageThree.enabled = true;
                ChallengeThreeIconChecker.enabled = true;
            }
            else
            {
                _challengeImageThree.enabled = false;
                ChallengeThreeIconChecker.enabled = false;
            }
        }
    }

}
