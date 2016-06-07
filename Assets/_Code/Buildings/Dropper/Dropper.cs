using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents a element that drops something when it dies.
    /// </summary>
    public class Dropper : MonoBehaviour
    {
        public GameObject _toDrop = null;
        public int _hp = 3;

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Character character;
            if ((character = collision.gameObject.GetComponent<Character>()) != null)
            {
				EffectManager.Instance.CreateWallHitEffect (this.transform);
				EffectManager.Instance.CreateDieEffect (character.transform);

				character.HP -= 1;
                this._hp -= character.Damage;
                if (this._hp <= 0)
                {
                    this.Die();
                }
            }
        }

        public void Die()
        {
			EffectManager.Instance.CreateSmokeEffect (this.transform);
            GameObject.Instantiate(this._toDrop, this.transform.position, this.transform.rotation);
            GameObject.Destroy(this.gameObject);
        }
    }
}