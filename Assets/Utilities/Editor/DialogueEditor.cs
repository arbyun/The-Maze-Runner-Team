using Dialogue;
using Dialogue.Data;
using Dialogue.Models;
using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    public class DialogueEditor
    {
        [MenuItem("TMZ/Setup Scene", false, 2)]
        public static void Setup()
        {
            Graphfl.Stm();
            var prevDialogManager = Object.FindObjectOfType<DialogueManager>();
            if (prevDialogManager == null)
            {
                var g = new GameObject("Dialog Manager");
                g.AddComponent<DialogueManager>();
            }

            GraphSaveUtilities.GenerateFolders();
            GraphSaveUtilities.CreateFirstCharacter("Player");
        }

        [MenuItem("TMZ/Create/New CharacterData", false, 1)]
        public static void CreateCharacter()
        {
            Graphfl.Stm();
            if (!Graphfl.Graphl2L()) return;

            var character = ScriptableObject.CreateInstance<CharacterData>();

            GraphSaveUtilities.GenerateFolders();

            AssetDatabase.CreateAsset(character,
                $"{GameConstants.FolderCharactersComplete}/New CharacterData.asset");
            EditorUtility.SetDirty(character);
            AssetDatabase.SaveAssets();

            Selection.activeObject = character;
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("TMZ/Dialogue/Create/New Interaction", false, 1)]
        public static void CreateInteraction()
        {
            Graphfl.Stm();
            var interaction = ScriptableObject.CreateInstance<Interaction>();

            GraphSaveUtilities.GenerateFolders();

            AssetDatabase.CreateAsset(interaction,
                $"{GameConstants.FolderInteractionsComplete}/New Interaction.asset");
            EditorUtility.SetDirty(interaction);
            AssetDatabase.SaveAssets();

            Selection.activeObject = interaction;
            SceneView.FrameLastActiveSceneView();
        }
    }
}