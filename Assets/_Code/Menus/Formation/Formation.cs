using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Code.Game;
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

		private HashSet<int> UsedIndexes = new HashSet<int>();

        public GameObject GridCellPrefab = null;

		public bool HasSpace { get { return UsedIndexes.Count < Buckets.Count; } }

        public Text TextNameLevel;
        public Text TextNameSoldier;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {

            var soldiers = GameManager.Instance.GetAvailableSoldiersRepresentation();
            //var soldiers =
            //    GameManager.Instance._allSoldiers.Select(
            //        g => Instantiate(GameManager.Instance.GetRepresentationOfSoldier(g.GetComponent<Soldier>()._id))).ToList();

            var grid = this.GetComponent<GridLayoutGroup>();
            var size = grid.GetComponent<RectTransform>().sizeDelta;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2()
            {
                x = size.x,
                y = ((grid.cellSize.y + grid.spacing.y) * Mathf.CeilToInt(soldiers.Count / grid.constraintCount + 1)) + .5f,
            };

            for (int i = 0; i < soldiers.Count; i++)
            {
                var gridCell = (GameObject) Instantiate(GridCellPrefab);
                gridCell.transform.SetParent(this.transform);
                gridCell.GetComponent<TroopCell>().Soldier = soldiers[i].GetComponent<Soldier>();
            }

            this.BattleButton.enabled = false;
            this.TextNameLevel.text = GameManager.Instance.CurrentLevel.Name;
        }

        public void AddToFormation(Soldier soldier, GameObject representation)
        {
			if (!HasSpace)
                return;

			int index = 0;
			while (UsedIndexes.Contains (index))
				index++;
			UsedIndexes.Add (index);
			representation.transform.SetParent(this.Buckets[index], false);
            //representation.transform.position = Vector3.zero;

            var rect = representation.GetComponent<RectTransform>();
            
            this.Soldiers.Add(soldier);

            if (Soldiers.Count > 0)
                this.BattleButton.enabled = true;
        }

        public void RemoveFromFormation(Soldier soldier, GameObject representation, bool destroy = true)
        {
			this.UsedIndexes.Remove (this.Buckets.IndexOf (representation.transform.parent));

            representation.transform.SetParent(null);

            this.Soldiers.Remove(soldier);

            if (Soldiers.Count == 0)
                this.BattleButton.enabled = false;

            Destroy(representation);
        }

        public void Battle()
        {
			
            GameManager.Instance.NextLevelSoldiersDefinition.Clear();

            for (int i = 0; i < this.Soldiers.Count; i++)
            {
                GameManager.Instance.NextLevelSoldiersDefinition.Add(new KeyValuePair<int, string>(Soldiers[i]._id, Soldiers[i]._soldierName));
            }

			string sceneName = GameManager.Instance.CurrentLevel.SceneName;
						
            GameManager.Instance.LoadScene(sceneName);
			AudioManager.Instance.PlayInterfaceSound (2);
        }

        public void ShowName(string name)
        {
            TextNameSoldier.text = name;
        }

        public void ShowNameIndex(int index)
        {
            this.ShowName(this.Buckets[index].GetComponentInChildren<Soldier>()._soldierName);
        }

        public void Return()
        {
			AudioManager.Instance.PlayInterfaceSound (0);
            GameManager.Instance.LoadScene("MapMenu");
        }
    }
}
