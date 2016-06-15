using UnityEngine;
using System.Collections;
using System.Linq;
using Assets._Code.Game;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the end level scene.
    /// </summary>
    public class WinLevelPanel : MonoBehaviour
    {
        public static WinLevelPanel Instance;

        public Image _starOne;
        public Image _starTwo;
        public Image _starThree;

        public GameObject ScrollParent = null;

        public GameObject NewSoldierPrefab = null;

        public void Awake()
        {
            Instance = this;
        }

        public void UpdateInfo(LevelInformation levelInfo)
        {
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

            foreach (var soldierIds in levelInfo.ChallengesCompleted)
            {
                if (GameManager.Instance.ChallengesComplete.Contains(soldierIds.Key.Id))
                    continue;

                for (int i = 0; i < soldierIds.Value.Length; i++)
                {
                    var parent = (GameObject) Instantiate(this.NewSoldierPrefab);
                    parent.transform.SetParent(this.ScrollParent.transform, false);

                    GameObject repre = (GameObject)Instantiate(GameManager.Instance.GetRepresentationOfSoldier(soldierIds.Value[i]));
                    repre.transform.SetParent(parent.transform, false);
                }
            }
        }

        public void Show()
        {
            LevelManager.Instance.WinPanelShown = true;
            LevelManager.Instance.PauseGame();
            LevelManager.Instance.FadeInCollider(null);
            this.transform.DOMove(LevelManager.Instance._shownPositionPanels.transform.position, .5f);
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            LevelManager.Instance.UnpauseGame();
            LevelManager.Instance.FadeOutCollider(null);
            LevelManager.Instance.WinPanelShown = false;
            this.transform.DOMove(LevelManager.Instance._hidenPositionPanels.transform.position, .5f);
            this.gameObject.SetActive(false);
        }
    }
};