using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace BounceDudes
{
    public class SoldierArray : MonoBehaviour
    {
        public static SoldierArray Instance;
        public Dictionary<int, List<GameObject>> Soldiers = new Dictionary<int, List<GameObject>>();
        public Soldier First;
        
        public void Start()
        {
            Instance = this;
            var soldiers = GameManager.Instance.GetAvailableSoldiersRepresentation();
            foreach (var soldier in soldiers)
            {
                GameObject soldierRepresentation = soldier;

                soldierRepresentation.transform.SetParent(this.transform);

                Soldier soldierScript = soldier.GetComponent<Soldier>();

                if (!Soldiers.ContainsKey(soldierScript._id))
                {
                    Soldiers.Add(soldierScript._id, new List<GameObject>());
                }
                Soldiers[soldier.GetComponent<Soldier>()._id].Add(soldierRepresentation);
                if (First == null)
                    First = soldierRepresentation.GetComponent<Soldier>();
            }
        }

        public int GetInstanceId(Soldier soldier)
        {
            return Soldiers[soldier._id].IndexOf(soldier.gameObject);
        }
    }
}
