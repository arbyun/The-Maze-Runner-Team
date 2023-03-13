using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    /// <summary>
    /// The actual drawer for the ConditionalHideAttribute script
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Override the OnGui method to draw our own drawer
        /// </summary>
        /// <param name="position">Self explanatory</param>
        /// <param name="property">Used to get the ConditionalHideAttribute from the property</param>
        /// <param name="label">Label to be shown</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            // Check if the attribute condition is met
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property); 
 
            // store the prev enabled state of the GUI
            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            
            // only draw if it's not hidden or if it's shown but the att condition is met
            if (!condHAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
 
            // restore the prev enabled state of the GUI
            GUI.enabled = wasEnabled;
        }
 
        /// <summary>
        /// Override of the GetPropertyHeight method
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
 
            if (!condHAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            // otherwise, return a height of -standardVerticalSpacing to hide the property
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
 
        /// <summary>
        /// Helper to get the value of the att condition
        /// </summary>
        /// <param name="condHAtt"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;
            // get the path of the property we want to apply the attribute to
            string propertyPath = property.propertyPath; 
            // replaces the path to the conditional source property one
            string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
            // finds the serialized property of the conditional source property
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
 
            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }
 
            return enabled;
        }
    }
}
