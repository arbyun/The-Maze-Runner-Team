using UnityEngine;

namespace Utilities
{
    public class FadeOut : MonoBehaviour
    {
        public float fadeTime = 1f; // Time it takes for the object to fade out
        private SpriteRenderer[] _spriteRenderer;
        private bool _fade;

        private void Start()
        {
            _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (_fade)
            {
               foreach (var children in _spriteRenderer)
               {
                   Color currentColor = children.color;
                   float fadeAmount = currentColor.a - (Time.deltaTime / fadeTime);
                   
                   // Clamp the fadeAmount between 0 and 1
                   fadeAmount = Mathf.Clamp01(fadeAmount);
   
                   // Set the new color with the updated alpha value
                   children.color = new Color(currentColor.r, currentColor.g, currentColor.b, fadeAmount);
               } 
            }
        }
    }
}