using System;
using Dialogue.Data;

namespace Dialogue.Nodes
{
    [Serializable]
    public class DialogueContent
    {
        public string characterID;
        public string dialogText = "New Dialog";

        public void Fill(DialogueNode dialogueNode)
        {
            characterID = dialogueNode.Content.characterID;
            dialogText = dialogueNode.Content.dialogText;
        }
    }
}