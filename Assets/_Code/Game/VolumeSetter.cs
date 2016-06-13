using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using UnityEngine;

namespace BounceDudes
{
    public class VolumeSetter : MonoBehaviour
    {
        public bool IsSound = false;

        public void Start()
        {
            if (this.IsSound)
                GetComponent<AudioSource>().volume = GameManager.Instance.SoundVolume;
            else
                GetComponent<AudioSource>().volume = GameManager.Instance.SoundVolume;
        }
    }
}
