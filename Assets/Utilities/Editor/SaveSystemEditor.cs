using GameSystems.Inventory;
using UnityEditor;

namespace Utilities.Editor
{
    [CustomEditor(typeof(SaveSystem))]
    [CanEditMultipleObjects]
    public class SaveSystemEditor : UnityEditor.Editor
    {
        private SerializedProperty _itemLibrary;

        private void OnEnable()
        {
            _itemLibrary = serializedObject.FindProperty("itemLibrary");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Put all the items that exist in this List");
            EditorGUILayout.LabelField("The items ID must correspond with the place on the List");
            EditorGUILayout.LabelField("Example: Big Axe ID is 0 so it must be placed in element 0");


            serializedObject.Update();
            EditorGUILayout.PropertyField(_itemLibrary);
            serializedObject.ApplyModifiedProperties();
        }
    }
}