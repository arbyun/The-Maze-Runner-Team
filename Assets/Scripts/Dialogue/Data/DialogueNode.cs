using System.Collections.Generic;
using Dialogue.Models;
using Dialogue.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Data
{
    public class DialogueNode : Node
    {
        protected DialogueGraphView GraphView;
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public List<string> Choices { get; set; }

        public NodeTypes DialogType { get; set; }

        public DialogueContent Content { get; set; }

        protected static List<CharacterData> AllCharacters => DataUtilities.GetAllCharacters();

        public void Init(DialogueGraphView graphView, DialogueNodeData nodeData, NodeTypes type)
        {
            GraphView = graphView;
            Choices = nodeData.choices;
            Content = nodeData.content;
            Guid = nodeData.guid;
            DialogType = type;

            SetPosition(new Rect(nodeData.position, Vector2.zero));

            mainContainer.AddToClassList("prata-node_main-container");
            extensionContainer.AddToClassList("prata-node_extension-container");
        }

        public virtual void Init(DialogueGraphView graphView, Vector2 position, NodeTypes type)
        {
            GraphView = graphView;
            Choices = new List<string>();
            Content = new DialogueContent
            {
                characterID = AllCharacters[0].id
            };
            DialogType = type;

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("prata-node_main-container");
            extensionContainer.AddToClassList("prata-node_extension-container");
        }

        public virtual void Draw()
        {
            title = Content.dialogText;

            // create the characterData who is talking
            var characterSelector = NodeElementsUtilities.CreateDropDownMenu("Characters");

            characterSelector.RegisterValueChangedCallback(evt =>
            {
                var index = AllCharacters.FindIndex(character => character.characterName == evt.newValue);
                Content.characterID = AllCharacters[index].id;
            });

            characterSelector.AppendCharacterAction(AllCharacters, Content.characterID,
                action => { characterSelector.text = ((CharacterData)action.userData).characterName; });
            mainContainer.Insert(1, characterSelector);

            // Input

            var inputPort = this.CreatePort("Dialog Connection", direction: Direction.Input,
                capacity: Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            // Foldout
            var customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("prata-node_custom-data-container");

            var textFoldout = NodeElementsUtilities.CreateFoldout("Dialog text");

            var textTextField = NodeElementsUtilities.CreateTextField(Content.dialogText, evt =>
            {
                Content.dialogText = evt.newValue;
                title = evt.newValue;
            });

            textTextField.AddClasses("prata-node_textfield",
                "prata-node_quote-textfield"
            );

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        public void RemoveFromChoices(string choice)
        {
            Choices.Remove(choice);
        }
    }
}