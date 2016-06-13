using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    [Serializable]
    public class Achivment
    {
        public AchivmentId Id;
        public Sprite AchivmentImage;
        public string Name;
        public string Description;
    }
}
