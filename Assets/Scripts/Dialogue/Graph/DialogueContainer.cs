using System;
using System.Collections.Generic;
using Dialogue.Nodes;
using UnityEngine;

namespace Dialogue.Graph
{
    [Serializable]
    public class DialogueContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogNodes = new List<DialogueNodeData>();
    }
}