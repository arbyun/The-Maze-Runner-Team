using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    public class DragReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Tween targets will be color-faded when drag events occure.
        /// </summary>
        public List<Image> tweenTargets;

        /// <summary>
        /// If drop is allowed, the spot will be faded with this color.
        /// </summary>
        public Color colorDropAllowed = new Color(0.5f, 1f, 0.5f);

        /// <summary>
        /// If drop is not allowed, the spot will be faded with this color.
        /// </summary>
        public Color colorDropDenied = new Color(1f, 0.5f, 0.5f);

        /// <summary>
        /// Group values are used to deny drop items to the same container.
        /// </summary>
        public string group;

        /// <summary>
        /// Filter items that can be dropped to this drag receiver by type.
        /// </summary>
        public List<ItemType> itemTypes;

        /// <summary>
        /// Filter items that can be dropped to this drag receiver by tags.
        /// </summary>
        public List<ItemTags> itemTags;

        /// <summary>
        /// Becomes [true] when drag was started and mouse position is over this drag receiver.
        /// </summary>
        public static bool DropReady;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryItem.DragTarget == null || InventoryItem.DragTarget.group == group) return;

            if (itemTypes.Any() && !itemTypes.Contains(InventoryItem.DragTarget.item.itemType))
            {
                Fade(colorDropDenied);
            }
            else if (itemTags.Any(i => !InventoryItem.DragTarget.item.tags.Contains(i)))
            {
                Fade(colorDropDenied);
            }
            else
            {
                Fade(colorDropAllowed);
                DropReady = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DropReady = false;
            Fade(Color.white);
        }

        private void Fade(Color color)
        {
            tweenTargets.ForEach(i => i.CrossFadeColor(color, 0.25f, true, false));
        }
    }
}