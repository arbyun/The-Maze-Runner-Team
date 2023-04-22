using UnityEditor;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Game manager class for in editor changing the layer of a game object after runtime based on
    /// certain conditions
    /// </summary>
    public class LayerRenderingHelper : MonoBehaviour
    {
        [Header("Layer to change")]
        public LayerMask layerMask;

        [Header("GameObject you want to change layers")]
        public GameObject gameObjectToChange;
        
        [Header("Change layer based on...")]
        
        [Header("Time")]
        public bool useTimer;
        
        [ConditionalHide("useTimer", true)] 
        public float timer;
        
        [Header("GameObject (trigger on collision)")]
        public bool useGameObject;
        
        [ConditionalHide("useGameObject", true)]
        public GameObject gameObj;

        /// <summary>
        /// Made a mask field in the editor to more easily select the layer the g.o. will
        /// change to. The function is called when the object is selected in the editor.</summary> 
        /// <returns> A layer mask.</returns>
        private void OnDrawGizmosSelected()
        {
            // created array of strings that will hold the layer names
            string[] layerNames = new string[32];
            
            for (int i = 0; i < 32; i++)
            {
                // get the layer name
                string layerName = LayerMask.LayerToName(i);
                // if the layer name is empty just give it a default name
                layerNames[i] = string.IsNullOrEmpty(layerName) ? ("Layer " + i) : layerName;
            }
            layerMask = EditorGUILayout.MaskField("Layer", layerMask.value, layerNames);
        }

        /// <summary> Checks if the timer has been reached, and if so, changes the layer of gameObjectToChange to
        /// layerMask.</summary>
        private void Update()
        {
            if (useTimer && Time.time > timer)
            {
                gameObjectToChange.layer = layerMask;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (useGameObject && other.gameObject == gameObj)
            {
                gameObjectToChange.layer = layerMask;
            }
        }
    }
}