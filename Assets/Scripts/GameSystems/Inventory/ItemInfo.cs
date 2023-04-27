using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    public class ItemInfo: MonoBehaviour
    {
        public Text name;
        public Text description;
        public Image icon;

        public void Reset()
        {
            name.text = description.text = null;
            //icon.sprite = ImageCollection.Instance.DefaultItemIcon;
        }

        public void Initialize(Guid itemId, List<ItemTags> itemParams, ItemType type, bool shop = false)
        {
            //icon.sprite = ImageCollection.Instance.GetIcon(itemId);
            name.text = SplitName(itemId.ToString());
            this.description.text = $"Here will be {itemId} description soon...";

            var description = new List<string> {$"Type: {type}"};

            if (itemParams.Any())
            {
                description[^1] += $" <color=grey>[{string.Join(", ", itemParams.Select(i => $"{i}").ToArray())}" +
                                   $"]</color>";
            }

            foreach (var attribute in itemParams)
            {
                description.Add($"Tag: {SplitName(attribute.ToString())}");
            }

            this.description.text = string.Join(Environment.NewLine, description.ToArray());
        }
        
        public static string SplitName(string name)
        {
            return Regex.Replace(Regex.Replace(name, "[A-Z]", " $0"), "([a-z])([1-9])", 
                "$1 $2").Trim();
        }
    }
}