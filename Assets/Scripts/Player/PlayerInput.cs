using System;
using System.ComponentModel;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public bool Interacting { get; private set; }
        public KeyCode Jump { get; set; } = KeyCode.Space;
        public KeyCode Interact { get; set; } = KeyCode.I;
        public KeyCode Attack { get; set; } = KeyCode.Z;
        public KeyCode Dash { get; set; } = KeyCode.LeftShift;

        private void Update()
        {
            Interacting = Input.GetKeyDown(Interact);
        }
        

        /// <summary> The ChangeKey function changes the value of a KeyCode variable to a new KeyCode.</summary>
        /// <param name="originalKey"> The original key to be changed.</param>
        /// <param name="newKey"> The new key to be assigned. </param>
        public void ChangeKey(ref KeyCode originalKey, KeyCode newKey)
        {
            if (!Enum.IsDefined(typeof(KeyCode), originalKey))
                throw new InvalidEnumArgumentException(nameof(originalKey), (int)originalKey, typeof(KeyCode));

            originalKey = newKey;
        }
    }
}