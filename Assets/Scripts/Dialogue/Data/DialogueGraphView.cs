using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue.Models;
using Dialogue.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Dialogue.Data
{
    public class DialogueGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
        private readonly DialogueEditorWindow _editorWindow;
        private DialogueSearchWindow _searchWindow;

        public DialogueGraphView(DialogueEditorWindow dialogueEditorWindow)
        {
            _editorWindow = dialogueEditorWindow;
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            AddStyles();

            // Creating start node
            AddElement(GenerateEntryPointNode());
        }

        private DialogueNode GenerateEntryPointNode()
        {
            var node = new DialogueNode
            {
                title = "Start",
                DialogType = NodeTypes.Start
            };

            node.SetPosition(new Rect(100, 200, 100, 150));

            var outputPort = node.CreatePort("Next");
            node.outputContainer.Add(outputPort);

            // we can't remove or move the first node
            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            node.RefreshExpandedState();
            node.RefreshPorts();

            return node;
        }

        #region Override Methods

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;

                if (startPort.node == port.node) return;

                if (startPort.direction == port.direction) return;

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        #endregion

        #region Element Creation

        public DialogueNode RestoreNode(DialogueNodeData nodeData)
        {
            var nodeType = Type.GetType($"{GameConstants.NodeTypeBase}{nodeData.type}Node");

            if (nodeType == null) return null;

            var node = (DialogueNode)Activator.CreateInstance(nodeType);

            node.Init(this, nodeData, nodeData.type);
            node.Draw();
            node.RefreshExpandedState();
            node.RefreshPorts();

            return node;
        }

        public DialogueNode CreateNode(NodeTypes type, Vector2 mousePosition)
        {
            var nodeType = Type.GetType($"{GameConstants.NodeTypeBase}{type}Node");

            if (nodeType == null) return null;

            var node = (DialogueNode)Activator.CreateInstance(nodeType);

            node.Init(this, mousePosition, type);
            node.Draw();
            node.RefreshExpandedState();
            node.RefreshPorts();

            return node;
        }

        public Group CreateGroup(string title, Vector2 mousePosition)
        {
            var group = new Group
            {
                title = title
            };

            group.SetPosition(new Rect(mousePosition, Vector2.zero));

            return group;
        }

        #endregion

        #region Manipulators

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());

            this.AddManipulator(CreateNodeContextualMenu("Add Single Choice Node", NodeTypes.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Choice Node", NodeTypes.MultipleChoice));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, NodeTypes type)
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    actionTitle,
                    actionEvent => AddElement(
                        CreateNode(type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
                    )
                )
            );

            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Create Group",
                    actionEvent => AddElement(
                        CreateGroup("Dialog Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
                    )
                )
            );

            return contextualMenuManipulator;
        }

        #endregion

        #region Element Addition

        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();
                _searchWindow.Init(this);
            }

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(GameConstants.StylesheetGraph,
                GameConstants.StylesheetNode
            );
        }

        private void AddGridBackground()
        {
            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        #endregion

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            var worldMousePosition = mousePosition;

            if (isSearchWindow) worldMousePosition -= _editorWindow.position.position;

            return contentViewContainer.WorldToLocal(worldMousePosition);
        }

        public void RemovePort(DialogueNode node, Port socket)
        {
            if (node.Choices.Count > 1)
            {
                node.RemoveFromChoices(socket.portName);
                var targetEdge = edges.ToList()
                    .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
                var enumerable = targetEdge as Edge[] ?? targetEdge.ToArray();
                if (enumerable.Any())
                {
                    var edge = enumerable.First();
                    edge.input.Disconnect(edge);
                    RemoveElement(enumerable.First());
                }

                node.outputContainer.Remove(socket);
                node.RefreshPorts();
                node.RefreshExpandedState();
            }
        }

        #endregion
    }
}