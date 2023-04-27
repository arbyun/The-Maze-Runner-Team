using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Models
{
    [Serializable]
    public class DialogueData : ScriptableObject
    {
        public string guid;
        public CharacterData characterData;
        public string text;
        public bool isFacingRight = true;
        public List<string> choices;
    }
}