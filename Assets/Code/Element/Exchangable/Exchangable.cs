using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for all elements that can exchange from enemy to player and vice versa.
    /// </summary>
    public class Exchangable : MonoBehaviour
    {
        private GameObject _currentOcuppant = null;
        
        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == TagAndLayer.PLAYER_OBJECTS || collider.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                if (this._currentOcuppant != null)
                {
                    this.Remove(this._currentOcuppant);
                }
                this.Add(collider.gameObject);
                this._currentOcuppant = gameObject;
            }
        }

        virtual public void Remove(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();
            character.RigidBody.WakeUp();
        }

        virtual public void Add(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();
            character.RigidBody.Sleep();
        }
    }
}