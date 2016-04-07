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
            if (collision.gameObject.layer == TagAndLayer.ENEMY_OBJECTS)
            {
                LevelManager.Instance._enemyBase.HP -= this._regen;
                this.Die();
            }
            else if (collision.gameObject.layer == TagAndLayer.PLAYER_OBJECTS)
            {
                LevelManager.Instance._playerBase.HP += this._regen;
                this.Die();
            }
        }

        public void Die()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}