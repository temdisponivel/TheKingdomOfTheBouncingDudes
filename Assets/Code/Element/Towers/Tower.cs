using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Base class for the tower element of game play.
    /// </summary>
    abstract public class Tower : MonoBehaviour
    {
        [Header("Attributes")]
        [Tooltip("Whether this tower can change teams.")]
        public bool _isDynamic = true;

        protected float _innerAureateSize = .5f;

        protected GameObject _currentOccupant = null;

        virtual public void OnTriggerEnter2D(Collider2D collider)
        {
        	
        }

        virtual public void OnTriggerStay2D(Collider2D collider)
        {
            Character character = null;
            if (Vector2.Distance(collider.gameObject.transform.position, this.transform.position) >= this._innerAureateSize 
                && (character = collider.gameObject.GetComponent<Character>()) != null && character.AffectedByElement)
            {
                this.AffectObject(collider.gameObject);
            }
        }

        /// <summary>
        /// Function called when this tower should affect a game object.
        /// </summary>
        abstract public void AffectObject(GameObject affected);

   
    }
}