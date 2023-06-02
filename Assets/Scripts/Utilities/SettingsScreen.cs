
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class SettingsScreen : MonoBehaviour
    {
        public Dictionary<string, GameObject> keyBindings = new Dictionary<string, GameObject>();

        public Text messageText;
        public SettingsManager settingsManager;
    
        public GameObject keyBindingPanel;
        public Button closeButton;
    
        private bool _isListeningForKey = false;

        void Start()
        {
            keyBindingPanel.SetActive(false);
            closeButton.onClick.AddListener(closeSettingsScreen);
            
            foreach (string keyName in SettingsManager.defaultKeys.Keys)
            {
                // Find the UI element with the corresponding name
                GameObject keyBindingUI = transform.Find(keyName).gameObject;

                // Store the UI element in the dictionary
                keyBindings.Add(keyName, keyBindingUI);

                // Update the UI element with the current key binding
                KeyCode keyCode = SettingsManager.defaultKeys[keyName];
                keyBindingUI.GetComponentInChildren<Text>().text = keyCode.ToString();
            }

            messageText.text = "";
        }



        /*IEnumerator listenForKeyPress(GameObject buttonPressed)
        {
            GameObject keyBindingUI = keyBindings[buttonPressed.name];
            KeyCode key = SettingsManager.defaultKeys[buttonPressed.name];

            // Update the UI element with the current key binding
            keyBindingUI.GetComponentInChildren<Text>().text = key.ToString();

            // Display a message to the player
            messageText.text = "Press any key to bind to " + key;

            // Wait for a key to be pressed
            while (!Input.anyKeyDown)
            {
                yield return null;
            }

            // Get the key that was pressed
            KeyCode newKeyCode = KeyCode.None;

            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    newKeyCode = keyCode;
                    break;
                }
            }

            // Update the key binding in the SettingsManager's defaultKeys dictionary
            if (newKeyCode != KeyCode.None)
            {
                SettingsManager.defaultKeys.Values[key] = newKeyCode;
            }
        }*/

        // Opens the key binding panel and starts listening for a new interact key
        public void openKeyBindingPanel()
        {
            keyBindingPanel.SetActive(true);
            _isListeningForKey = true;
        }

        // Closes the key binding panel
        public void closeKeyBindingPanel()
        {
            keyBindingPanel.SetActive(false);
            _isListeningForKey = false;
        }

        // Closes the settings screen
        public void closeSettingsScreen()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isListeningForKey)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        foreach (string keyName in keyBindings.Keys)
                        {
                            GameObject keyBindingUI = keyBindings[keyName];
                            if (keyBindingUI == null)
                            {
                                continue;
                            }

                            if (keyBindingUI.name == keyCode.ToString())
                            {
                                continue;
                            }

                            if (keyBindings[keyName].GetComponentInChildren<Text>().text == keyCode.ToString())
                            {
                                keyBindings[keyName].GetComponentInChildren<Text>().text = 
                                    SettingsManager.defaultKeys[keyName].ToString();
                                SettingsManager.defaultKeys[keyName] = keyCode;
                                break;
                            }
                        }

                        _isListeningForKey = false;
                        keyBindingPanel.SetActive(false);
                        messageText.text = "";
                        break;
                    }
                }
            }
        }
    }
}
