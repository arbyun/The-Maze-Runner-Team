using System.Collections.Generic;
using System.Linq;
using Dialogue.Data;
using UnityEngine;

namespace Dialogue.Models
{
    public static class DataUtilities
    {
        public static bool HasAnyCharacter()
        {
            return GetAllCharacters().Any();
        }

        public static List<CharacterData> GetAllCharacters()
        {
            var characters =
                Resources.LoadAll<CharacterData>(GameConstants.FolderCharacters);
            return characters.ToList();
        }
    }
}