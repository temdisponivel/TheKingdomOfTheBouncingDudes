using UnityEngine;
using System.Collections;

namespace BounceDudes
{
    /// <summary>
    /// Class that represents the base of the player.
    /// </summary>
    public class Base : MonoBehaviour
    {
        public int _hp = 10;
        public int HP { get { return _hp; } set { _hp = value; if (_hp <= 0) { LevelManager.Instance.GameOver(); } } }
    }
}