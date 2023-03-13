using System;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Attribute class that can be used to hide/show a field or property in the Unity Editor based on a bool
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public string ConditionalSourceField;
        public bool HideInInspector = false;
 
        /// <summary>
        /// Constructor that gets the bool by it's variable string name
        /// </summary>
        /// <param name="conditionalSourceField">The bool field in control</param>
        public ConditionalHideAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
        }
 
        /// <summary>
        /// Constructor that takes a flag indicating whether to hide or not field in
        /// inspector
        /// </summary>
        /// <param name="conditionalSourceField">The bool field in control</param>
        /// <param name="hideInInspector">True/False</param>
        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
        }
    }
}
