using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BounceDudes
{
    public class Formation : MonoBehaviour
    {
        public static Formation Instance;

        public Button BattleButton = null;

        public List<Soldier> Soldiers = new List<Soldier>();

        public List<Transform> Buckets = new List<Transform>();

        private Stack<int> FreeIndexes = new Stack<int>();

        public GameObject GridCellPrefab = null;

        public bool HasSpace { get { return FreeIndexes.Count != 0; } }

        public Text TextNameSoldier;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            for (int i = this.Buckets.Count-1; i >= 0; i--)
                this.FreeIndexes.Push(i);

            var soldiers = GameManager.Instance.GetAvailableSoldiersRepresentation();
            //var soldiers =
            //    GameManager.Instance._allSoldiers.Select(
            //        g => Instantiate(GameManager.Instance.GetRepresentationOfSoldier(g.GetComponent<Soldier>()._id))).ToList();

            var grid = this.GetComponent<GridLayoutGroup>();
            var size = grid.GetComponent<RectTransform>().sizeDelta;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2()
            {
                x = size.x,
                y = ((grid.cellSize.y + grid.spacing.y) * soldiers.Count / grid.constraintCount) + .5f,
            };

            for (int i = 0; i < soldiers.Count; i++)
            {
                var gridCell = (GameObject) Instantiate(GridCellPrefab);
                gridCell.transform.SetParent(this.transform);
                gridCell.GetComponent<TroopCell>().Soldier = soldiers[i].GetComponent<Soldier>();
            }

            this.BattleButton.enabled = false;
        }

        public void AddToFormation(Soldier soldier, GameObject representation)
        {
            if (FreeIndexes.Count == 0)
                return;

            representation.transform.SetParent(this.Buckets[FreeIndexes.Pop()], false);
            //representation.transform.position = Vector3.zero;

            var rect = representation.GetComponent<RectTransform>();
            
            this.Soldiers.Add(soldier);

            if (Soldiers.Count > 0)
                this.BattleButton.enabled = true;
        }

        public void RemoveFromFormation(Soldier soldier, GameObject representation, bool destroy = true)
        {
            this.FreeIndexes.Push(this.Buckets.IndexOf(representation.transform.parent));

            representation.transform.SetParent(null);

            this.Soldiers.Remove(soldier);

            if (Soldiers.Count == 0)
                this.BattleButton.enabled = false;

            Destroy(representation);
        }

        public void Battle()
        {
			AudioManager.Instance.PlayInterfaceSound (2);
            GameManager.Instance.NextLevelSoldiersDefinition.Clear();

            for (int i = 0; i < this.Soldiers.Count; i++)
            {
                GameManager.Instance.NextLevelSoldiersDefinition.Add(new KeyValuePair<int, string>(Soldiers[i]._id, Soldiers[i]._soldierName));
            }

			string sceneName = GameManager.Instance.CurrentLevel.SceneName;

			AudioManager.Instance.PlayMusic(sceneName == "LevelBOSS" ? 3 : 2);
            GameManager.Instance.LoadScene(sceneName);
        }

        public void ShowName(string name)
        {
            TextNameSoldier.text = name;
        }

        public void ShowNameIndex(int index)
        {
            this.ShowName(this.Buckets[index].GetComponentInChildren<Soldier>()._soldierName);
        }
    }
}
