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
        public ScrollRect Scroll;
        public DiaryController _diary;
        public LevelNode _lastNode;

		public Sprite StarBorderTransparent;
		public Sprite StarBorderBronze;
        public Sprite StarBorderSilver;
        public Sprite StarBorderGold;

        public static MapMenu Instance = null;

		protected bool _pressedEscape = false;

        public void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            LevelNode[] nodes = this.GetComponentsInChildren<LevelNode>();
            LevelInformation[] levelsUnlocked = GameManager.Instance.LevelsInformation.Values.ToArray();

            levelsUnlocked.OrderBy(l => (int) l.LevelId);
            int lastLevel = -1; //-1 to sum 0 below

            //int current = 0;

            if (levelsUnlocked.Length > 0)
            {
                if (levelsUnlocked[levelsUnlocked.Length - 1].Finished)
                    lastLevel = (int)levelsUnlocked[levelsUnlocked.Length - 1].LevelId;
            }

            foreach (var node in nodes)
            {
                if ((int) node.LevelId > lastLevel + 1)
                {
					node._levelImage.color *= (Color.gray);
                    node.gameObject.GetComponent<Button>().enabled = false;
					node.gameObject.GetComponent<LevelNode> ().setLocked(true);
                }
            }

			if (!GameManager.Instance.FromMainMenuToMapMenu) {
				var percent = 1f / nodes.Length;
				Scroll.normalizedPosition = Vector2.one * percent * levelsUnlocked.Length;
			}

			GameManager.Instance.FromMainMenuToMapMenu = false;


        }

		void Update(){
			if (Input.GetKeyDown(KeyCode.Escape) && !_pressedEscape)
			{
				_pressedEscape = true;
				this.Return ();
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

			AudioManager.Instance.PlayInterfaceSound (3);
        }

        public void HideDiary()
        {
            _diary.Hide();
			AudioManager.Instance.PlayInterfaceSound (4);
        }

        public void PlayNode()
        {
			AudioManager.Instance.PlayInterfaceSound (2);
            GameManager.Instance.CurrentLevel = GameManager.Instance.LevelsById[_lastNode.LevelId];
            GameManager.Instance.LoadScene("FormationMenu");
            //SceneManager.LoadScene("FormationMenu");
            //SceneManager.LoadScene(GameManager.Instance.LevelsById[_lastNode.LevelId].SceneName);
        }

        public void Return()
        {
			AudioManager.Instance.PlayInterfaceSound (0);
            GameManager.Instance.LoadScene("TitleScreen");
        }

		public void Troop()
		{
			AudioManager.Instance.PlayInterfaceSound (0);
			GameManager.Instance.LoadScene("TroopMenu");
			//SceneManager.LoadScene("TroopMenu");
		}
    }

}
