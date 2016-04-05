using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a object that regenerates another object's life.
    /// </summary>
    public class LifeRegen : MonoBehaviour
    {
        public int _regen = 1;
        public void OnCollisionEnter2D(Collision2D collision)
        {
            Character character;
            if ((character = collision.gameObject.GetComponent<Character>()) != null)
            {
                character.HP += this._regen;
                this.Die();
            }
        }

        public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}