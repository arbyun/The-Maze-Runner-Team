using System.Collections.Generic;
using System.Linq;
using Dialogue.Graph;
using UnityEngine;

namespace Dialogue.Models
{
    [CreateAssetMenu(fileName = "Interaction", menuName = "TMZ/Create", order = 0)]
    public class Interaction : ScriptableObject
    {
        public DialogueContainer dialogueContainer;

        private readonly List<DialogueData> _conversation = new List<DialogueData>();

        private DialogueData _currentDialogue;
        private bool _hasStarted;

        private List<CharacterData> AllCharacters => DataUtilities.GetAllCharacters();

        public void Reset()
        {
            if (!_hasStarted) return;

            _currentDialogue = null;
            _hasStarted = false;
        }

        private void OnEnable()
        {
            if (dialogueContainer == null || AllCharacters == null || AllCharacters.Count <= 0) return;

            var lastCharacter = dialogueContainer.DialogNodes[0].content.characterID;

            // by default we start facing right
            var isFacingRight = true;
            foreach (var nodeData in dialogueContainer.DialogNodes)
            {
                // this means we have a connection (it's a valid Node)
                var dialogueData = new DialogueData
                {
                    guid = nodeData.guid
                };

                var character =
                    AllCharacters.Find(character => character.id == nodeData.content.characterID);

                dialogueData.characterData = character;
                dialogueData.text = nodeData.content.dialogText;

                // If the characterData that's speaking changes, then we toggle it so it faces left
                if (lastCharacter != character.id)
                {
                    isFacingRight = !isFacingRight;
                    lastCharacter = character.id;
                }

                dialogueData.isFacingRight = isFacingRight;

                // we need to only show the choices that actually have a connection on the port
                var usedChoices = dialogueContainer.NodeLinks
                    .Where(node => node.baseNodeGuid == nodeData.guid)
                    .Select(node => node.portName)
                    .ToList();

                dialogueData.choices = usedChoices;

                _conversation.Add(dialogueData);
            }

            _currentDialogue = null;
            _hasStarted = false;
        }

        public DialogueData GetCurrentDialogue()
        {
            if (_currentDialogue == null)
            {
                _currentDialogue = _conversation[0];
                _hasStarted = true;
                return _currentDialogue;
            }

            _currentDialogue = GetNextDialogue(_currentDialogue.guid);
            return _currentDialogue;
        }

        public DialogueData GetCurrentDialogueFromChoice(string dialogueGuid, string choice)
        {
            _currentDialogue = GetNextDialogue(dialogueGuid, choice);
            return _currentDialogue;
        }

        private DialogueData GetNextDialogue(string guid)
        {
            var linkData = dialogueContainer.NodeLinks
                .FirstOrDefault(link => link.baseNodeGuid == guid);
            var nodeData = dialogueContainer.DialogNodes.FirstOrDefault(node => node.guid == linkData?.targetNodeGuid);
            var next = _conversation.FirstOrDefault(dialogueData => dialogueData.guid == nodeData?.guid);

            return next;
        }

        private DialogueData GetNextDialogue(string guid, string choice)
        {
            var linkData = dialogueContainer.NodeLinks
                .FirstOrDefault(link => link.baseNodeGuid == guid && link.portName == choice);
            var nodeData = dialogueContainer.DialogNodes.FirstOrDefault(node => node.guid == linkData?.targetNodeGuid);
            var next = _conversation.FirstOrDefault(dialogueData => dialogueData.guid == nodeData?.guid);

            return next;
        }

        public bool HasAnyDialogueLeft()
        {
            if (!_hasStarted) return true;

            return _currentDialogue != null &&
                   dialogueContainer.NodeLinks.Any(link => link.baseNodeGuid == _currentDialogue.guid);
        }
    }
}