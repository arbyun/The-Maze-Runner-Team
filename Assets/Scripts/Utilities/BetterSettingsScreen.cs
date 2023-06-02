using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class BetterSettingsScreen : MonoBehaviour
    {
        public SettingsManager settingsManager; // Reference to the SettingsManager script

        public GameObject keyBindingButtonPrefab; // Prefab for the key binding button
        public Transform keyBindingButtonContainer; // Container for the key binding buttons

        private Dictionary<string, KeyCode> _currentKeys; // Dictionary for storing the current keys

        private bool _waitingForKey = false; // Flag for whether we're waiting for a key press to rebind a key
        private KeyCode _keyToRebind; // The key we're waiting to rebind
        private string _nameOfKeyToRebind;

        void Start()
        {
            _currentKeys = SettingsManager.defaultKeys; // Initialize currentKeys with the defaultKeys from SettingsManager

            // Iterate through each key in currentKeys and create a key binding button for it
            foreach (KeyValuePair<string, KeyCode> pair in _currentKeys)
            {
                GameObject newButton = Instantiate(keyBindingButtonPrefab, keyBindingButtonContainer);
                newButton.GetComponentInChildren<Text>().text = pair.Key + " Key: " + pair.Value.ToString(); // Set the button text
                Button buttonComponent = newButton.GetComponent<Button>(); // Get the button component
                string keyName = pair.Key; // Get the key name (used in the lambda expression below)
                buttonComponent.onClick.AddListener(() => startRebindingKey(keyName)); // Add a listener to start rebinding the key
            }
        }

        private static string getKeyFromValue(KeyCode valueVar, Dictionary<string, KeyCode> dictionaryVar)
        {
            foreach (string keyVar in dictionaryVar.Keys)
            {
                if (dictionaryVar[keyVar] == valueVar)
                {
                    return keyVar;
                }
            }
            return null;
        }

        void Update()
        {
            // If we're waiting for a key press to rebind a key...
            if (_waitingForKey && Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    // If the current key being pressed matches a valid KeyCode...
                    if (Input.GetKeyDown(keyCode))
                    {
                        _nameOfKeyToRebind = getKeyFromValue(keyCode, _currentKeys);
                        // Update the currentKeys dictionary with the new key binding
                        _currentKeys[_nameOfKeyToRebind] = keyCode;

                        // Update the key binding button text
                        foreach (Transform child in keyBindingButtonContainer)
                        {
                            if (child.GetComponentInChildren<Text>().text.Contains(_nameOfKeyToRebind))
                            {
                                child.GetComponentInChildren<Text>().text = _nameOfKeyToRebind + " Key: " + keyCode.ToString();
                                break;
                            }
                        }

                        // Stop waiting for a key press
                        _waitingForKey = false;
                        _keyToRebind = KeyCode.None;
                        _nameOfKeyToRebind = null;

                        break;
                    }
                }
            }
        }

        // Function to start rebinding a key
        public void startRebindingKey(string keyName)
        {
            // Set the keyToRebind to the key name clicked
            _keyToRebind = (KeyCode)System.Enum.Parse(typeof(KeyCode), _currentKeys[keyName].ToString());
            _waitingForKey = true; // Set the waitingForKey flag to true
        }
    }
}