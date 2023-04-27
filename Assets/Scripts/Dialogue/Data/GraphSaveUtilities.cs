using System.Collections.Generic;
using System.Linq;
using Dialogue.Graph;
using Dialogue.Models;
using Dialogue.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Data
{
    public class GraphSaveUtilities
    {
        private DialogueContainer _containerCached;
        private DialogueGraphView _targetGraphView;

        private List<Edge> Edges => _targetGraphView.edges.ToList();

        private Dictionary<string, DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>()
            .ToDictionary(node => node.Guid, node => node);

        public static GraphSaveUtilities GetInstance(DialogueGraphView graphView)
        {
            return new GraphSaveUtilities
            {
                _targetGraphView = graphView
            };
        }

        public void SaveGraph(string filename)
        {
            var path = $"{GameConstants.FolderGraph}/{filename}.asset";
            var prevAsset = Resources.Load<DialogueContainer>(path);
            if (prevAsset == null)
                SaveNewGraph(path);
            else
                OverwriteGraph(prevAsset);
        }

        private void SaveNewGraph(string path)
        {
            Debug.Log($"New graph {path}");
            var dialogContainer = ScriptableObject.CreateInstance<DialogueContainer>();

            SavePorts(dialogContainer);

            SaveNodes(dialogContainer);

            // creates an Resources folder if there's none
            GenerateFolders();

            AssetDatabase.CreateAsset(dialogContainer, path);
            EditorUtility.SetDirty(dialogContainer);
            AssetDatabase.SaveAssets();

            Selection.activeObject = dialogContainer;
            SceneView.FrameLastActiveSceneView();
        }

        public void OverwriteGraph(DialogueContainer dialogContainer)
        {
            Debug.Log($"Overwriting {AssetDatabase.GetAssetPath(dialogContainer)}");
            SavePorts(dialogContainer);

            SaveNodes(dialogContainer);

            EditorUtility.SetDirty(dialogContainer);
            AssetDatabase.SaveAssets();

            Selection.activeObject = dialogContainer;
            SceneView.FrameLastActiveSceneView();
        }

        private void SaveNodes(DialogueContainer dialogContainer)
        {
            foreach (var dialogueNode in Nodes.Values.Where(node => node.DialogType != NodeTypes.Start))
            {
                var dialogNode = new DialogueNodeData(dialogueNode);
                var index = dialogContainer.DialogNodes.FindIndex(p => p.guid == dialogueNode.Guid);
                if (index >= 0)
                    // update
                    dialogContainer.DialogNodes[index] = dialogNode;
                else
                    // new one
                    dialogContainer.DialogNodes.Add(dialogNode);
            }
        }

        private void SavePorts(DialogueContainer dialogContainer)
        {
            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            foreach (var connectedPort in connectedPorts)
            {
                if (connectedPort.output.node is DialogueNode outputNode && connectedPort.input.node is DialogueNode inputNode)
                {
                    var linkData = new NodeLinkData
                    {
                        baseNodeGuid = outputNode.Guid,
                        portName = connectedPort.output.portName,
                        targetNodeGuid = inputNode.Guid
                    };

                    var index = dialogContainer.NodeLinks
                        .FindIndex(l => l.baseNodeGuid == outputNode.Guid &&
                                        l.targetNodeGuid == inputNode.Guid);

                    if (index >= 0)
                        // update
                        dialogContainer.NodeLinks[index] = linkData;
                    else
                        // new one
                        dialogContainer.NodeLinks.Add(linkData);
                }
            }
        }

        public void LoadGraph(DialogueContainer dialogContainer)
        {
            _containerCached = dialogContainer;
            if (_containerCached == null)
            {
                EditorUtility.DisplayDialog("File not found", "Target dialog graph does not exist", "Ok");
                return;
            }

            ClearGraph();

            RestoreNodes();

            ConnectNodes();
        }

        public void ClearAll()
        {
            _targetGraphView.DeleteElements(Nodes.Values.Where(t => t.DialogType != NodeTypes.Start));
            // remove edges connected to node
            _targetGraphView.DeleteElements(Edges);
        }

        private void ClearGraph()
        {
            // set the entry point
            Nodes.Values.First(x => x.DialogType == NodeTypes.Start).Guid =
                _containerCached.NodeLinks[0].baseNodeGuid;

            ClearAll();
        }

        private void RestoreNodes()
        {
            foreach (var nodeData in _containerCached.DialogNodes)
            {
                var tempNode = _targetGraphView.RestoreNode(nodeData);
                _targetGraphView.AddElement(tempNode);
            }
        }

        private void ConnectNodes()
        {
            foreach (var node in Nodes.Values)
            {
                var connections = _containerCached.NodeLinks.Where(
                    x => x.baseNodeGuid == node.Guid).ToList();

                foreach (var nodeLinkData in connections)
                {
                    var targetNodeGuid = nodeLinkData.targetNodeGuid;
                    var targetNode = Nodes.Values.FirstOrDefault(x => x.Guid == targetNodeGuid);

                    if (targetNode == null) continue;

                    // we search for the corresponding port name
                    var outputPort = node.outputContainer.Children()
                        .FirstOrDefault(child => child.Q<Port>().portName.Equals(nodeLinkData.portName))
                        .Q<Port>();

                    if (outputPort != null)
                    {
                        LinkNodesTogether(outputPort, (Port)targetNode.inputContainer[0]);

                        targetNode.SetPosition(new Rect(
                            _containerCached.DialogNodes.First(x => x.guid == targetNodeGuid).position,
                            _targetGraphView.DefaultNodeSize));
                    }
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge
            {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        public static void GenerateFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder(GameConstants.FolderGraph))
                AssetDatabase.CreateFolder("Assets/Resources", "Graphs");

            if (!AssetDatabase.IsValidFolder(GameConstants.FolderInteractionsComplete))
                AssetDatabase.CreateFolder("Assets/Resources", "Interactions");

            if (!AssetDatabase.IsValidFolder(GameConstants.FolderCharactersComplete))
                AssetDatabase.CreateFolder("Assets/Resources", "Characters");
        }

        public static void CreateFirstCharacter(string newName = "")
        {
            // only if this is the first characterData we create a dummy one
            if (!DataUtilities.HasAnyCharacter()) return;

            if (!AssetDatabase.IsValidFolder(GameConstants.FolderCharactersComplete))
                AssetDatabase.CreateFolder("Assets/Resources", "Characters");

            var character = ScriptableObject.CreateInstance<CharacterData>();

            character.characterName = newName;

            AssetDatabase.CreateAsset(character,
                $"{GameConstants.FolderCharactersComplete}/New CharacterData.asset");
            EditorUtility.SetDirty(character);
            AssetDatabase.SaveAssets();
        }
    }
}