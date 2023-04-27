using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    [RequireComponent(typeof(DragReceiver))]
    public class ScrollInventory : ItemContainer
    {
        public ScrollRect scrollRect;
        public GridLayoutGroup grid;
        public GameObject itemPrefab;
        public GameObject cellPrefab;
        public int minRows;
        public int viewportOffset; // When scrollbar becomes visible

        private readonly List<ItemType> _sorting = new List<ItemType>
        {
            ItemType.Buff,
            ItemType.Consumable,
            ItemType.Weapon
        };

        private List<Guid> _hash = new List<Guid>();
        private readonly List<InventoryItem> _items = new List<InventoryItem>();

        public new void Initialize(ref List<Item> items)
        {
            base.Initialize(ref items);
        }
        
        public override void Refresh()
        {
            if (Items.Any(i => i.Count <= 0))
            {
                throw new Exception(string.Join(", ", Items.Where(i => i.Count <= 0).Select(i => 
                    i.id.ToString()).ToArray()));
            }

            var refresh = Items.Select(i => i.id).SequenceEqual(_hash);

            if (refresh && _items.Any())
            {
                foreach (var button in _items)
                {
                    button.count.text = button.item.Count.ToString();
                }
            }
            else
            {
                Reset();
            }
        }

        private void Reset()
        {
            _hash = Items.Select(i => i.id).ToList();
            _items.Clear();

            foreach (Transform child in grid.transform)
            {
                Destroy(child.gameObject);
            }

            var items = Items.OrderBy(i => _sorting.Contains(i.itemType) ? _sorting.IndexOf(i.itemType) : 
                0).ToList();
            var groups = items.GroupBy(i => i.itemType);

            items = new List<Item>();

            foreach (var group in groups)
            {
                items.AddRange(group.OrderBy(i => i.itemType));
            }

            var dragReceiver = GetComponent<DragReceiver>();

            foreach (var item in items)
            {
                var button = Instantiate(itemPrefab, grid.transform).GetComponent<InventoryItem>();

                button.item = item;
                button.count.text = item.Count.ToString();
                button.group = dragReceiver.group;
                _items.Add(button);
            }

            if (grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount && minRows > 0)
            {
                var columns = grid.constraintCount;
                var rows = Mathf.Max(minRows, Mathf.CeilToInt((float)items.Count / columns));

                for (var i = items.Count; i < columns * rows; i++)
                {
                    Instantiate(cellPrefab, grid.transform);
                }
            }

            StartCoroutine(ResetScrollRect());
        }

        private IEnumerator ResetScrollRect()
        {
            yield return null;

            scrollRect.verticalNormalizedPosition = 1;
            scrollRect.horizontalNormalizedPosition = 0;

            yield return null;

            FixUnityScrollRectBug();
        }

        /// <summary>
        /// ScrollRect viewport should be shifted a little bit left when vertical scrollbar becomes visible.
        /// This behaviour should be handled by ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport, but it doesn't work on devices.
        /// Bug: https://goo.gl/tkr6Vs
        /// The workaround does the job.
        /// </summary>
        private void FixUnityScrollRectBug()
        {
            var scrollbar = scrollRect.verticalScrollbar;
            var offset = scrollbar.IsActive() ? viewportOffset : 0;

            scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                scrollRect.GetComponent<RectTransform>().rect.width - offset);
        }
    }
}