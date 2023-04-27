﻿using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    public class ItemInfoUpdate : MonoBehaviour
    {
   
        public GameObject infoPanel;
        public Text nameText;
        public Image icon;

        public void UpdateInfoPanel(Item itemInfo)
        {
            if (itemInfo != null)
            {
                infoPanel.SetActive(true);

                nameText.text = itemInfo.itemName;
                icon.sprite = itemInfo.itemIcon;
            }
            else
            {
                infoPanel.SetActive(false);
            }
        }

        public void ClosePanel()
        {
            infoPanel.SetActive(false);
        }
    }
}
