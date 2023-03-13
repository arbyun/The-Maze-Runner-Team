using UnityEngine;

namespace Systems
{
    public abstract class InputData
    {
        private InputData(KeyCode jumpKey, KeyCode inventoryKey, KeyCode attackKey, KeyCode escapeKey, KeyCode rightKey, KeyCode leftKey, KeyCode dashKey)
        {
            this.JumpKey = jumpKey;
            this.InventoryKey = inventoryKey;
            this.AttackKey = attackKey;
            this.EscapeKey = escapeKey;
            this.RightKey = rightKey;
            this.LeftKey = leftKey;
            this.DashKey = dashKey;
        }

        private KeyCode JumpKey { get; set; }
        private KeyCode DashKey { get; set; }
        private KeyCode LeftKey { get; set; }
        private KeyCode RightKey { get; set; }
        private KeyCode EscapeKey { get; set; }
        private KeyCode AttackKey { get; set; }
        private KeyCode InventoryKey { get; set; }
    }

    public class InputManager : MonoBehaviour
    {
        public InputData InputKeys;
    }
}