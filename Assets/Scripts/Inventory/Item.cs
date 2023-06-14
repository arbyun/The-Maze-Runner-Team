using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "TMZ/New.../ItemData", order = 1)]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite itemInventoryImage;
        public Sprite itemInGameImage;
        public GameObject itemGameObject;
        public bool stackable;
        public int quantity = 1;
        public bool equippable;
        public Guid id = new Guid();
        public ItemType[] types; [Tooltip("Choose only two at max.")]

        internal void Validate()
        {
            if (!itemGameObject)
            {
                if (!Directory.Exists("Assets/Prefabs"))
                    AssetDatabase.CreateFolder("Assets", "Prefabs");

                string localPath = "Assets/Prefabs/" + itemName + ".prefab";

                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                bool tempWeapon = false;
                bool tempConsumable = false;

                GameObject gm = new GameObject($"{itemName}");

                if (types.Length > 1)
                {
                    foreach (var type in types)
                    {
                        if (type == ItemType.Weapon)
                        {
                            tempWeapon = true;
                        }
                        else if (type == ItemType.Consumable)
                        {
                            tempConsumable = true;
                        }
                    }
                }

                if (tempConsumable)
                {
                    gm.AddComponent<Consumable>();
                }

                if (tempWeapon)
                {
                    gm.AddComponent<Weapon>();
                }

                PrefabUtility.SaveAsPrefabAsset(gm, localPath, out bool idk);

                itemGameObject = gm;
            }
        }
    }
}