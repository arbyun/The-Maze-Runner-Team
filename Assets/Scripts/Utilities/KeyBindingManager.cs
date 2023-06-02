using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    public class KeyBindingManager: IDataType
    {
        public KeyCode interact = KeyCode.D;
        public KeyCode jump = KeyCode.Space;
        public KeyCode switchWeapon = KeyCode.X;
        public KeyCode dash = KeyCode.LeftShift;
        public KeyCode attack = KeyCode.Z;
        public KeyCode pause = KeyCode.Escape;
        public KeyCode inventory = KeyCode.I;
    }
}