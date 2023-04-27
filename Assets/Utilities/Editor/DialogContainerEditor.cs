using System.Collections.Generic;
using System.Linq;
using Dialogue.Graph;
using Dialogue.Models;
using Dialogue.Nodes;
using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    [CustomEditor(typeof(DialogueContainer))]
    public class DialogContainerEditor : UnityEditor.Editor
    {
        private const string Space = "    ";

        private Dictionary<string, List<NodeLinkData>> _allLinks;

        private DialogueContainer _dialogContainer;
        private readonly Dictionary<string, bool> _folded = new();
        private static List<CharacterData> AllCharacters => DataUtilities.GetAllCharacters();

        private void OnEnable()
        {
            _dialogContainer = (DialogueContainer)target;
            _allLinks = GetNodeLinksBasedOnId(_dialogContainer.NodeLinks);

            foreach (var nodeLink in _allLinks)
                _folded[nodeLink.Key] = true;
        }

        public override void OnInspectorGUI()
        {
            DrawNodeLinks(_dialogContainer.NodeLinks, _dialogContainer.DialogNodes);
        }

        private void DrawNodeLinks(List<NodeLinkData> nodeLinks, List<DialogueNodeData> dialogNodes)
        {
            // we need to flatten the list because a link can have multiple choices       

            var nodeIndex = 1;

            var nodeTitleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold
            };

            var italicStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(),
                fontStyle = FontStyle.Italic
            };

            var characterSayingStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(),
                padding = new RectOffset(32, 32, 0, 0)
            };

            foreach (var nodeLink in _allLinks)
            {
                var parent = GetDialogNodeFromLinkData(nodeLink.Key, dialogNodes);

                GUIContent parentTitle;
                if (parent == null)
                {
                    parentTitle = new GUIContent($"{nodeIndex} Start");
                }
                else
                {
                    var characterName = GetCharacterName(parent.characterID);
                    parentTitle = new GUIContent(
                        $"{nodeIndex} [{characterName}]: " + parent.dialogText
                    );
                }

                _folded[nodeLink.Key] =
                    EditorGUILayout.BeginFoldoutHeaderGroup(_folded[nodeLink.Key], parentTitle, nodeTitleStyle);

                if (_folded[nodeLink.Key])
                {
                    var rect = EditorGUILayout.BeginVertical();

                    var option = 1;

                    foreach (var linkData in nodeLink.Value)
                    {
                        EditorGUILayout.LabelField($"{Space}-> [Option {option}] " + linkData.portName, italicStyle);

                        // next one
                        var dialogContent = GetDialogNodeFromLinkData(linkData.targetNodeGuid, dialogNodes);

                        EditorGUILayout.LabelField(GetDialogHeader(dialogContent), characterSayingStyle);
                        EditorGUILayout.Separator();
                        //GUILayout.FlexibleSpace();

                        option++;
                    }

                    EditorGUILayout.EndVertical();
                    GUI.Box(rect, GUIContent.none);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                nodeIndex++;
            }
        }

        private GUIContent GetDialogHeader(DialogueContent dialogContent)
        {
            var characterName = GetCharacterName(dialogContent.characterID);

            return new GUIContent(
                $" [{characterName}]: " + dialogContent.dialogText,
                $"({characterName}: {dialogContent.dialogText}"
            );
        }

        private Dictionary<string, List<NodeLinkData>> GetNodeLinksBasedOnId(List<NodeLinkData> nodeLinks)
        {
            return nodeLinks
                .GroupBy(nodeLink => nodeLink.baseNodeGuid)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());
        }

        private DialogueContent GetDialogNodeFromLinkData(string baseGUID, List<DialogueNodeData> dialogNodes)
        {
            return dialogNodes.Find(nodeData => nodeData.guid.Equals(baseGUID))?.content;
        }

        private string GetCharacterName(string characterGuid)
        {
            var content = AllCharacters.Find(character => character.id.Equals(characterGuid)).characterName;

            if (string.IsNullOrEmpty(content))
                return "name Not Set";
            return content;
        }
    }
}