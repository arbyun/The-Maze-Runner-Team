namespace GameSystems.Inventory
{
    [System.Serializable]
    public class InventoryData
    {
        public string inventoryString;

        public InventoryData(string invStr)
        {
            inventoryString = invStr;
        }
    }
}
