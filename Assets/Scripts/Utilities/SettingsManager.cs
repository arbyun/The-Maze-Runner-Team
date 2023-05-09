using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class SettingsManager : MonoBehaviour
    {
        public KeyBindingManager keyBindings;
        
        // This is the UI text element that will display the current key bindings
        public TextMeshProUGUI keyBindingText;

        // This is the UI panel that will be displayed when the player is rebinding keys
        public GameObject rebindPanel;

        // This is the name of the action that is currently being rebound
        private string _rebindingAction;

        private Dictionary<string, KeyCode> _actionKeys;
        internal static Dictionary<string, KeyCode> defaultKeys;

        // Start is called before the first frame update
        void Start()
        {
            _actionKeys = new Dictionary<string, KeyCode>();
            defaultKeys = new Dictionary<string, KeyCode>
            {
                // Add default values for each action
                { "Interact", keyBindings.interact},
                { "Jump", keyBindings.jump},
                { "Attack", keyBindings.attack},
                { "Dash", keyBindings.dash},
                { "Switch Weapon", keyBindings.switchWeapon},
                { "Open Inventory", keyBindings.inventory},
                { "Pause", keyBindings.pause}
            };

            // Copy default keys to action keys
            copyDictionary(defaultKeys, _actionKeys);

            // Update the key binding text
            updateKeyBindingText();
        }

        // Update is called once per frame
        void Update()
        {
            // If the player is rebinding keys...
            if (_rebindingAction != null)
            {
                // Check if any key is pressed
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        // Save the new key binding and stop rebinding
                        if (_rebindingAction != null) _actionKeys[_rebindingAction] = keyCode;
                        _rebindingAction = null;

                        // Update the key binding text
                        updateKeyBindingText();

                        // Hide the rebind panel
                        rebindPanel.SetActive(false);
                    }
                }
            }
        }

        // This method updates the key binding text with the current key bindings
        void updateKeyBindingText()
        {
            // Clear the text
            keyBindingText.text = "";

            // Add a line for each action name and key code
            foreach (string actionName in _actionKeys.Keys)
            {
                keyBindingText.text += actionName + ": " + _actionKeys[actionName].ToString() + "\n";
            }
        }

        // This method starts rebinding for the specified action name
        public void startRebinding(string actionName)
        {
            // Set the rebinding action
            _rebindingAction = actionName;

            // Show the rebind panel
            rebindPanel.SetActive(true);
        }

        // This method cancels rebinding
        public void cancelRebinding()
        {
            // Stop rebinding
            _rebindingAction = null;

            // Hide the rebind panel
            rebindPanel.SetActive(false);

            // Update the key binding text
            updateKeyBindingText();
        }

        // This method copies one dictionary to another
        static void copyDictionary<TK, TV>(Dictionary<TK, TV> source, Dictionary<TK, TV> target)
        {
            target.Clear();
            foreach (KeyValuePair<TK, TV> kvp in source)
            {
                target.Add(kvp.Key, kvp.Value);
            }
        }
    }
}