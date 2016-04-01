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
            //if triggered with the outer aureate
            if (Vector2.Distance(collider.gameObject.transform.position, this.transform.position) <= this._innerAureateSize)
            {
                if (this._currentOccupant != null && collider.gameObject.tag != this._currentOccupant.gameObject.tag)
                {
                    this.RemoveCurrentOccupant(this._currentOccupant);
                    this.AddOccupant(collider.gameObject);
                }
                else
                {
                    this.RemoveCurrentOccupant(collider.gameObject);
                } 
            }
        }

        virtual public void OnTriggerStay2D(Collider2D collider)
        {
            if (Vector2.Distance(collider.gameObject.transform.position, this.transform.position) >= this._innerAureateSize)
            {
                this.AffectObject(collider.gameObject);
            }
        }

        /// <summary>
        /// Function called when this tower should affect a game object.
        /// </summary>
        abstract public void AffectObject(GameObject affected);

        /// <summary>
        /// Funcion called when a game object is to be throwed out.
        /// </summary>
        /// <param name="occupant"></param>
        public void RemoveCurrentOccupant(GameObject occupant)
        {
        }

        /// <summary>
        /// Function called when a gameobject is to be set as occupant.
        /// </summary>
        /// <param name="newOccupant"></param>
        public void AddOccupant(GameObject newOccupant)
        {

        }
    }
}