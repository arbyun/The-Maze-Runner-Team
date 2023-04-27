using System.Collections.Generic;
using Dialogue.Models;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue.Data
{
    public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;
        private Texture2D _indentationIcon;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialog Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", _indentationIcon))
                {
                    level = 2,
                    userData = NodeTypes.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
                {
                    level = 2,
                    userData = NodeTypes.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialog Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", _indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (searchTreeEntry.userData)
            {
                case NodeTypes.SingleChoice:
                    var singleNode =
                        (DefaultDialogueNode)_graphView.CreateNode(NodeTypes.SingleChoice, localMousePosition);
                    _graphView.AddElement(singleNode);
                    return true;
                case NodeTypes.MultipleChoice:
                    var multipleNode =
                        (ChoiceDialogueNode)_graphView.CreateNode(NodeTypes.MultipleChoice, localMousePosition);
                    _graphView.AddElement(multipleNode);
                    return true;
                case Group _:
                    var group = _graphView.CreateGroup("Dialog Group", localMousePosition);
                    _graphView.AddElement(group);
                    return true;
                default:
                    return false;
            }
        }

        public void Init(DialogueGraphView dialogueGraphView)
        {
            _graphView = dialogueGraphView;
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }
    }
}