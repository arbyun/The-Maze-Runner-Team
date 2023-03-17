using UnityEngine;
using UnityEngine.InputSystem;


// TO BE REDONE!!!! USE UNITY'S INPUT SYSTEM



namespace GameSystems
{
    /// <summary>
    /// Class to define input keys
    /// </summary>
    public class InputData
    {
        /// <summary>
        /// Constructor that defines KeyCodes for each input key that will be necessary
        /// More can/will be added later
        /// </summary>
        /// <param name="jumpKey">Jump key</param>
        /// <param name="inventoryKey">Open inventory window in UI</param>
        /// <param name="attackKey">Attack with currently equipped weapon key</param>
        /// <param name="escapeKey">Pause, load or exit game (opens new UI window)</param>
        /// <param name="rightKey">Move left</param>
        /// <param name="leftKey">Move right</param>
        /// <param name="dashKey">Dash forwards</param>
        /// <param name="switchKey">Switch main weapon to secondary equipped weapon</param>
        internal InputData(KeyCode jumpKey, KeyCode inventoryKey, KeyCode attackKey, KeyCode escapeKey, KeyCode rightKey, 
            KeyCode leftKey, KeyCode dashKey, KeyCode switchKey)
        {
            this.JumpKey = jumpKey;
            this.InventoryKey = inventoryKey;
            this.AttackKey = attackKey;
            this.EscapeKey = escapeKey;
            this.RightKey = rightKey;
            this.LeftKey = leftKey;
            this.DashKey = dashKey;
            this.SwitchKey = switchKey;
        }

        public KeyCode JumpKey { get; set; }
        public KeyCode DashKey { get; set; }
        public KeyCode LeftKey { get; set; }
        public KeyCode RightKey { get; set; }
        public KeyCode EscapeKey { get; set; }
        public KeyCode AttackKey { get; set; }
        public KeyCode InventoryKey { get; set; }
        public KeyCode SwitchKey { get; set; }
    }

    /// <summary>
    /// Class to store an instance of InputData
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // We are also defining 'default' keys for each
        private const KeyCode DefaultJump = KeyCode.Space;
        private const KeyCode DefaultInventory = KeyCode.I;
        private const KeyCode DefaultAttack = KeyCode.A;
        private const KeyCode DefaultEscape = KeyCode.Escape;
        private const KeyCode DefaultRight = KeyCode.RightArrow;
        private const KeyCode DefaultLeft = KeyCode.LeftArrow;
        private const KeyCode DefaultDash = KeyCode.LeftShift;
        private const KeyCode DefaultSwitch = KeyCode.Z;
        public static InputData InputKeys = new InputData(DefaultJump, DefaultInventory, DefaultAttack, DefaultEscape, 
            DefaultRight, DefaultLeft, DefaultDash, DefaultSwitch);
        
        
        // Something to put in the Settings screen, allows for easy rebinding
        void RemapButtonClicked(InputAction actionToRebind)
        {
            var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                // To avoid accidental input from mouse motion
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .Start();
        }
    }
}