using System;
using UnityEngine;

namespace Dialogue.Models
{
    [CreateAssetMenu(fileName = "Character", menuName = "TMZ/Create", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();

        public string characterName;
    }
}