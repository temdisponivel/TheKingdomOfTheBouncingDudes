using UnityEngine;

namespace BounceDudes
{
    public class SoldierArray : MonoBehaviour
    {
        public void Start()
        {
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
            }
        }
    }
}
