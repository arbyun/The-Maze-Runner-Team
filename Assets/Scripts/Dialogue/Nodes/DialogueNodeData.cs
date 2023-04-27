using System;
using System.Collections.Generic;
using Dialogue.Data;
using Dialogue.Models;
using UnityEngine;

namespace Dialogue.Nodes
{
    [Serializable]
    public class DialogueNodeData
    {
        public string guid;
        public DialogueContent content;
        public Vector2 position;
        public NodeTypes type;
        public List<string> choices;

        public DialogueNodeData(DialogueNode dialogueNode)
        {
            var dialogContent = new DialogueContent();
            dialogContent.Fill(dialogueNode);

            guid = dialogueNode.Guid;
            this.content = dialogContent;
            position = dialogueNode.GetPosition().position;
            type = dialogueNode.DialogType;
            choices = dialogueNode.Choices;
        }
    }
}