using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (Item)target;
 
            if(GUILayout.Button("Generate Item", GUILayout.Height(20)))
            {
                script.Validate();
            }
         
        }
    }
}