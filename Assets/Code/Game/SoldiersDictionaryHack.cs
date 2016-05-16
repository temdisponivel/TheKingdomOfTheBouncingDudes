using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BounceDudes
{
    /// <summary>
    /// Just a clever hack to expose a dictionary-like structure in the inspector.
    /// </summary>
    [Serializable]
    public class SoldiersDictionaryHack
    {
        public int Star;
        public int[] SoldierToGive;
    }
}
