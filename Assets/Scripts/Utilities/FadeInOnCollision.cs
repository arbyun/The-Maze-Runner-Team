using System;
using UnityEngine;

namespace Utilities
{
    public class FadeInOnCollision : MonoBehaviour
    {
        public float fadeInTime = 1f; // Time it takes for the object to fade in
        public Color initialAlpha;
        public float maxAlpha = 0.8f;
        private SpriteRenderer _spriteRenderer;
        internal bool isFadingIn = false;
        internal bool isFadingOut = false;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // Set the initial alpha value to 0.5f
            Color currentColor = initialAlpha;
            _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        }

        private void Update()
        {

            if (isFadingIn)
            {
                Color currentColor = _spriteRenderer.color;
                float fadeAmount = currentColor.a + (Time.deltaTime / fadeInTime);
                

                // Clamp the fadeAmount between 0.5f and 1
                fadeAmount = Mathf.Clamp(fadeAmount, 0.5f, maxAlpha);

                // Set the new color with the updated alpha value
                _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, fadeAmount);

                // Stop fading in when the alpha value reaches 1
                if (fadeAmount == maxAlpha)
                {
                    isFadingIn = false;
                }
            }
            
            if (isFadingOut)
            {
                Debug.Log("isFadingOut: " + isFadingOut);
                Color currentColor = initialAlpha;
                float fadeAmount = initialAlpha.a + (Time.deltaTime / fadeInTime);

                // Clamp the fadeAmount between 0 and the maximum alpha value
                fadeAmount = Mathf.Clamp(fadeAmount, 0.5f, maxAlpha);

                // Set the new color with the updated alpha value
                _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, fadeAmount);

                // Destroy the object when the alpha value reaches 0
                if (_spriteRenderer.color == new Color(currentColor.r, currentColor.g, currentColor.b, initialAlpha.a))
                {
                    isFadingOut = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            isFadingOut = false;
            isFadingIn = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Trigger Exit");
            isFadingIn = false;
            isFadingOut = true;
            Debug.Log("isFadingOut: " + isFadingOut);
        }
    }
}