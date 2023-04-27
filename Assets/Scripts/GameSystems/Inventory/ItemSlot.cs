using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    [RequireComponent(typeof(DragReceiver))]
    public class ItemSlot : MonoBehaviour
    {
        public Image icon;
        public ItemType itemType;

        /// <summary>
        /// Filter compatible items by tags (for example filter weapons: sword, axe, dagger or bow).
        /// </summary>
        public List<ItemTags> itemTags;

        public void Start()
        {
            var dragReceiver = GetComponent<DragReceiver>();

            dragReceiver.itemTypes = new List<ItemType> { itemType };
            dragReceiver.itemTags = itemTags;
        }
    }
}