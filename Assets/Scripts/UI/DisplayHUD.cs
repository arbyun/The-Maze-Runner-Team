/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DisplayHUD : MonoBehaviour
    {
        private static readonly float CurrentHealth = Player.CurrentHealth;
        private static readonly float MaxHealth = Player.MaxHealth;
        private const float MaxSpeed = 10f;
        private static readonly float CurrentSpeed = Player.Speed;

        internal static Image HealthBarImage;
        private static Image[] _speedBlocks;
        private const float BlockWidth = 20f;
        private Color _ogSpeedBlockColor;
        private static Color _ogHealthBarColor;

        public GameObject healthBarFill;
        public GameObject speedBlockGroup;
        
        private void Awake()
        {
            HealthBarImage = healthBarFill.GetComponent<Image>();
            _speedBlocks = speedBlockGroup.GetComponentsInChildren<Image>();
            _ogSpeedBlockColor = _speedBlocks[0].color;
            _ogHealthBarColor = HealthBarImage.color;
        }

        /// <summary> Updates the health bar and speed blocks.</summary>
        private void Update()
        {
            HealthBarImage.fillAmount = CurrentHealth / MaxHealth;

            for (int i = 0; i < _speedBlocks.Length; i++)
            {
                float blockPosX = i * BlockWidth;
                float blockPosY = (i == 10) ? -40f : 0f;

                if (i > CurrentSpeed / MaxSpeed)
                {
                    StartCoroutine(FlashWhite(_speedBlocks[i]));
                }
                else
                {
                    _speedBlocks[i].rectTransform.anchoredPosition = new Vector2(blockPosX, blockPosY);
                    _speedBlocks[i].color = _ogSpeedBlockColor;
                }
                
                
            }
        }

        /// <summary> Used to flash the health bar white when the player takes damage.        
        /// </summary>
        /// <param name="image"> The image that will be flashed white </param>
        /// <returns> A ienumerator</returns>
        internal static IEnumerator FlashWhite(Image image)
        {
            Color color = image.color;
            image.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            if (image == HealthBarImage)
            { 
                image.color = _ogHealthBarColor;
            }
            else
            {
                for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.5f)
                {
                    Color transparent = new Color(color.r, color.g, color.b, 0f);
                    image.color = transparent;
                    yield return null;
                }

                image.color = new Color(color.r, color.g, color.b, 0f);
            }
            
        }
    }
}*/