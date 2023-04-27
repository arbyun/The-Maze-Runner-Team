using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image icon;
        public Image frame;
        public Text count;
        public Item item;
        public string group;

        private GameObject _phantom;
        private RectTransform _rect;
        private float _clickTime;

        public static InventoryItem DragTarget;
        public static Action<Item> OnDoubleClick;
        public static Action<Item> OnDragStarted;
        public static Action<Item> OnDragCompleted;
        public static Action<Item> OnItemSelected;

        public void Start()
        {
            //icon.sprite = ImageCollection.Instance.GetIcon(item.id);
        }

        /// <summary>
        /// Called from button script
        /// </summary>
        public void OnPress()
        {
            OnItemSelected?.Invoke(item);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnDoubleClick != null && Mathf.Abs(eventData.clickTime - _clickTime) < 0.5f) // If double click
            {
                OnDoubleClick(item);
            }

            _clickTime = eventData.clickTime;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //if (DragTarget != null || item.tags.Contains(ItemTags.NotForSale)) return;

            var canvas = FindInParents<Canvas>(gameObject);

            _phantom = Instantiate(gameObject, canvas.transform, true);
            _phantom.transform.SetAsLastSibling();
            _phantom.transform.localScale = transform.localScale;
            _phantom.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
            _phantom.GetComponent<CanvasGroup>().blocksRaycasts = false;
            _phantom.GetComponent<InventoryItem>().count.text = "1";
            _rect = canvas.GetComponent<RectTransform>();
            SetDraggedPosition(eventData);
            DragTarget = this;
            OnDragStarted?.Invoke(item);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            if (DragReceiver.DropReady)
            {
                OnDragCompleted?.Invoke(item);
            }

            DragTarget = null;
            DragReceiver.DropReady = false;
            Destroy(_phantom, 0.25f);

            foreach (var graphic in _phantom.GetComponentsInChildren<Graphic>())
            {
                graphic.CrossFadeAlpha(0f, 0.25f, true);
            }
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, data.position, data.pressEventCamera, 
                    out var mouse))
            {
                var rect = _phantom.GetComponent<RectTransform>();

                rect.position = mouse;
                rect.rotation = _rect.rotation;
            }
        }

        private static T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;

            var comp = go.GetComponent<T>();

            if (comp != null) return comp;

            var t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }

            return comp;
        }
    }
}