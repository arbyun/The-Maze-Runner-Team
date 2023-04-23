using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameSystems.Inventory
{
    public class SaveSystem : MonoBehaviour
    {
        // Place all items in the game by their ID order in this list(ID starts at 0)
        public List<Item> itemLibrary = new();

        // The string that is use to store the current Inventory Data when SaveInventory is called
        private string _inventoryString = "";

        public void TransformDataToString()
        {
            // For each item the script saves the ID and quantity of it
            foreach (var item in Inventory.Instance.itemList)
                _inventoryString = _inventoryString + item.id + ":" +
                                  Inventory.Instance.quantityList[Inventory.Instance.itemList.IndexOf(item)] + "/";
        }

        public void SaveInventory()
        {
            TransformDataToString();
            var destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            var data = new InventoryData(_inventoryString);
            var bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public void LoadInventory()
        {
            _inventoryString = "";
            var destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
            }
            else
            {
                Debug.LogError("File not found");
                return;
            }

            var bf = new BinaryFormatter();
            var data = (InventoryData)bf.Deserialize(file);
            file.Close();

            ReadInventoryData(data.inventoryString);

            // Update UI after Load
            Inventory.Instance.UpdateInventoryUI();
        }

        public void ReadInventoryData(string data)
        {
            Inventory.Instance.itemList.Clear();
            Inventory.Instance.quantityList.Clear();

            var splitData = data.Split(char.Parse("/"));

            foreach (var stg in splitData)
            {
                var splitID = stg.Split(char.Parse(":"));

                if (splitID.Length >= 2)
                {
                    Inventory.Instance.itemList.Add(itemLibrary[int.Parse(splitID[0])]);
                    Inventory.Instance.quantityList.Add(int.Parse(splitID[1]));
                }
            }
        }
    }
}