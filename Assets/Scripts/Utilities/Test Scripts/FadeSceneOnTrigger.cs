using UnityEngine;

namespace Utilities.Test_Scripts
{
    public class FadeSceneOnTrigger : MonoBehaviour
    {
        public Canvas canvas;
        public float fadeDuration = 5;
 
        private float maxAlpha = 1;
        private float startAlpha;
        private float canvasGroupAlpha;
        
        private float desiredAlpha;
        private float currentAlpha;
        public bool triggered;
        private CanvasGroup _canvasGroup;

        void Start()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }
 
        void Update()
        {
            currentAlpha = Mathf.MoveTowards( currentAlpha, desiredAlpha, 2.0f * Time.deltaTime);

            if (triggered)
            {
                Debug.Log("Triggered");
                trigger();
            }
        }

        private void trigger()
        {
            Debug.Log("TRIGGER");
            _canvasGroup.alpha = currentAlpha;
        }
    }
}