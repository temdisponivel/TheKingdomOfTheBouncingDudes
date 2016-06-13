using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BounceDudes
{
    public class SoldierArray : MonoBehaviour
    {
        public static SoldierArray Instance;
        private List<GameObject> Soldiers = new List<GameObject>();
        public void Start()
        {
            Instance = this;
            var soldiers = GameManager.Instance.GetAvailableSoldiers();

            foreach (var soldier in soldiers)
            {
                GameObject soldierRepresentation =
                    (GameObject)
                        GameObject.Instantiate(
                            GameManager.Instance.GetRepresentationOfSoldier(soldier.GetComponent<Character>()._id));
                soldierRepresentation.GetComponent<Soldier>()._soldierName =
                    soldier.GetComponent<Soldier>()._soldierName;
                soldierRepresentation.transform.SetParent(this.transform);
                Soldiers.Add(soldierRepresentation);
            }
        }

        public int GetInstanceId(Soldier soldier)
        {
            return Soldiers.IndexOf(soldier.gameObject);
        }
    }
}
