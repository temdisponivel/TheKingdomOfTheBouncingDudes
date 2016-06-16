using UnityEngine;
using System.Collections;
using System.Linq;
using Assets._Code.Game;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace BounceDudes
{

    /// <summary>
    /// Class that controls the Map Menu.
    /// </summary>
    public class MapMenu : MonoBehaviour
    {
        public DiaryController _diary;
        private LevelNode _lastNode;

        public void Start()
        {
            LevelNode[] nodes = this.GetComponentsInChildren<LevelNode>();
            LevelInformation[] levelsUnlocked = GameManager.Instance.LevelsInformation.Values.ToArray();

            levelsUnlocked.OrderBy(l => (int) l.LevelId);
            int lastLevel = -1; //-1 to sum 0 below

            if (levelsUnlocked.Length > 0)
            {
                if (levelsUnlocked[levelsUnlocked.Length - 1].Finished)
                    lastLevel = (int)levelsUnlocked[levelsUnlocked.Length - 1].LevelId;
            }

            foreach (var node in nodes)
            {
                if ((int) node.LevelId > lastLevel + 1)
                {
                    node._levelImage.color *= Color.gray;
                    node.gameObject.GetComponent<Button>().enabled = false;
                }
            }
        }

        public void SelectNode(LevelNode node)
        {
            if (_lastNode == node)
            {
                _diary.Hide();
                _lastNode = null;
                return;
            }
            _lastNode = node;
            _diary.UpdateInfo(node.LevelId);
            _diary.Show();
        }

        public void HideDiary()
        {
            _diary.Hide();
        }

        public void PlayNode()
        {
            GameManager.Instance.CurrentLevel = GameManager.Instance.LevelsById[_lastNode.LevelId];
            
            GameManager.Instance.LoadScene("FormationMenu");
            //SceneManager.LoadScene("FormationMenu");
            //SceneManager.LoadScene(GameManager.Instance.LevelsById[_lastNode.LevelId].SceneName);
        }

        public void Return()
        {
            GameManager.Instance.LoadScene("TitleScreen");
        }
    }

}
