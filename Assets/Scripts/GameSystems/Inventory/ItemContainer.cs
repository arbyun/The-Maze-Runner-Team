using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Inventory
{
    public abstract class ItemContainer : MonoBehaviour
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public List<Item> Items { get; protected set; }

        /// <summary>
        /// Either all items are expanded (i.e. item count = 1, so two equal items will be stored as two list elements).
        /// </summary>
        public bool expanded;

        public abstract void Refresh();

        public void Initialize(ref List<GameSystems.Inventory.Item> items)
        {
            Items = items;
            Refresh();
        }
    }
}