using System;
using UnityEngine;

namespace Utilities.Test_Scripts
{
    class GameManager: MonoBehaviour
    {
        public GameObject level;

        /*private void Awake()
        {
            foreach (Transform child in level.transform)
            {
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.AddComponent<FadeObj>();
                }
                
            }
            
            /*for (int i = 0; i < level.transform.childCount; i++)
            {
                Transform child = level.transform.GetChild(i);

                // Loop through each child of the current child
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform grandchild = child.GetChild(j);

                    // Add the FadeObj component to the grandchild's game object
                    grandchild.gameObject.AddComponent<FadeObj>();
                }
            }*/

            //level.transform.GetChild(childIndex).GetChild()
            //gameObject.AddComponent<FadeObj>();

        //}

        public void triggerEveryone()
        {
            foreach (Transform child in level.transform)
            {
                child.GetComponent<SendTriggerToChildren>().trigger();
            }
        }
    }
}