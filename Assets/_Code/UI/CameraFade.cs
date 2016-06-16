using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BounceDudes
{
    public class CameraFade : MonoBehaviour
    {
        public static CameraFade Instance = null;
        
        public SpriteRenderer ImageToFade;

        public void Awake()
        {
            Instance = this;

            if (this.ImageToFade == null)
                this.ImageToFade = this.GetComponent<SpriteRenderer>();

            this.FadeOut(null);
        }

        public void FadeOut(Action callback)
        {
            if (ImageToFade == null)
            {
                callback();
                return;
            }
            Color color = ImageToFade.color;
            color.a = 0;
            ImageToFade.DOBlendableColor(color, 1.5f).OnComplete(() =>
            {
                if (callback != null)
                    callback();
            });
        }

        public void FadeIn(Action callback)
        {
            if (ImageToFade == null)
            {
                callback();
                return;
            }

            Color color = ImageToFade.color;
            color.a = 1;
            ImageToFade.DOBlendableColor(color, 0.5f).OnComplete(() =>
            {
                if (callback != null)
                    callback();
            });
        }
    }
}
