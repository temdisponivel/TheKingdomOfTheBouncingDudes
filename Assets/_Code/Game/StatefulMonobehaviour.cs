using BounceDudes;
using UnityEngine;

namespace Assets.Code.Game
{
    public class StatefulMonobehaviour : MonoBehaviour
    {
        public void Update()
        {
            if (GameManager.Instance.State != GameState.PAUSED)
                this.InnerUpdate();
        }

        public virtual void InnerUpdate()
        {
        }
    }
}
