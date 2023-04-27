using Dialogue.Models;
using UnityEngine;

namespace Dialogue.Data
{
    public abstract class DefaultDialogueNode : DialogueNode
    {
        public override void Init(DialogueGraphView graphView, Vector2 position, NodeTypes type)
        {
            if (AllCharacters.Count <= 0)
            {
                Debug.LogError("Please be sure that you created at least 1 CharacterData");
                return;
            }

            base.Init(graphView, position, NodeTypes.SingleChoice);

            DialogType = NodeTypes.SingleChoice;
            Choices.Add("Next Dialog");
        }

        public override void Draw()
        {
            if (AllCharacters.Count <= 0)
            {
                Debug.LogError("Please be sure that you created at least 1 CharacterData");
                return;
            }

            base.Draw();
            foreach (var choice in Choices)
            {
                var portChoice = this.CreatePort(choice);

                outputContainer.Add(portChoice);
            }

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}