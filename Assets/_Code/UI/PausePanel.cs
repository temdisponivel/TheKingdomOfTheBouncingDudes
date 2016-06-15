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
    public class PausePanel : MonoBehaviour
    {
        public static PausePanel Instance;

        public void Show()
        {
            LevelManager.Instance.FadeInCollider(null);
            this.transform.DOMove(LevelManager.Instance._shownPositionPanels.transform.position, .5f);
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            LevelManager.Instance.FadeOutCollider(null);
            this.transform.DOMove(LevelManager.Instance._hidenPositionPanels.transform.position, .5f).OnComplete(() => { this.gameObject.SetActive(false); });
        }
    }
};