using System;
using System.Collections;
using UnityEngine;

namespace Utilities.Test_Scripts
{
    class FadeObj : MonoBehaviour
    {
        [Tooltip("Should the object fade in or fade out?")]
        public bool fadeIn, fadeOut;
        public float finalAlpha = 0f;
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (fadeIn)
            {
                var material = _renderer.material;
                Color c = material.color;
                c.a = finalAlpha;
                material.color = c;
            }
        }

        IEnumerator fadingOut()
        {
            for (float f = 1f; f >= -0.05; f -= 0.05f)
            {
                var material = _renderer.material;
                Color c = material.color;
                c.a = f;
                material.color = c;
                yield return new WaitForSeconds(0.05f);

                if (f >= finalAlpha)
                {
                    f = finalAlpha;
                }
            }
        }

        public void startFadingOut()
        {
            StartCoroutine(fadingOut());
        }

        IEnumerator fadingIn()
        {
            for (float f = 0.05f; f <= 1; f+= 0.05f)
            {
                var material = _renderer.material;
                Color c = material.color;
                c.a = f;
                material.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void startFadingIn()
        {
            StartCoroutine(fadingIn());
        }
    }
}