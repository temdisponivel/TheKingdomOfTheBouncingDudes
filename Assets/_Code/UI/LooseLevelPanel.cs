using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BounceDudes
{
    /// <summary>
    /// Class that handles the end level scene.
    /// </summary>
    public class LooseLevelPanel : MonoBehaviour
    {
        public static LooseLevelPanel Instance;

        public Text LooseText;

        public void Awake()
        {
            Instance = this;
        }

        public void UpdateInfo(LevelInformation levelInfo)
        {
            this.LooseText.text =
                GameManager.Instance.LooseGameMessages[Random.Range(0, GameManager.Instance.LooseGameMessages.Count)];
        }

        public void Show()
        {
            LevelManager.Instance.FadeInCollider(null);
            this.transform.DOMove(LevelManager.Instance._shownPositionPanels.transform.position, .5f);
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            LevelManager.Instance.FadeOutCollider(null);
            this.transform.DOMove(LevelManager.Instance._hidenPositionPanels.transform.position, .5f);
            this.gameObject.SetActive(false);
        }
    }
};