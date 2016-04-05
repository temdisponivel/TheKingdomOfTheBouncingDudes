using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BounceDudes
{
    /// <summary>
    /// Static class containing all tags.
    /// </summary>
    static public class TagAndLayer
    {
        public const string WALL = "Wall";
        public const string ELEMENT_DOOR = "ElementDoor";
        public const string BASE = "Base";
        public const string ENEMY_BASE = "EnemyBase";
        public const string PORTAL = "Portal";
        public const string TOWER = "Tower";

        public const int GAME_OBJECTS = 8;
        public const int PLAYER_OBJECTS = 9;
        public const int ENEMY_OBJECTS = 10;
    }
}
