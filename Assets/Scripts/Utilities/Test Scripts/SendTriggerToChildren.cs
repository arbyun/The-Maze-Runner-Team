using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Test_Scripts
{
    class SendTriggerToChildren : MonoBehaviour //and grandchildren
    {
        private FadeObj[] _children;
        //public bool fadeIn;
        //public bool fadeOut;

        private void Start()
        {
            foreach (Transform child in transform)
            {
                    child.gameObject.AddComponent<FadeObj>();
                    child.GetComponent<FadeObj>().finalAlpha = 0.5f;
            }

            List<FadeObj> childrenList = new List<FadeObj>();
            foreach (Transform child in transform)
            {
                FadeObj kid = child.GetComponent<FadeObj>();
                if (kid != null)
                    childrenList.Add(kid);
            }

            _children = childrenList.ToArray();
            Debug.Log("Children: " + _children.Length);
            
            /*List<FadeObj> childrenList = new List<FadeObj>();
            foreach (Transform child in transform)
            {
                FadeObj kid = child.GetComponent<FadeObj>();
                if (kid != null) // Check if FadeObj component exists before adding
                    childrenList.Add(kid);
            }

            _children = childrenList.ToArray(); // Convert the list back to an array
            Debug.Log("Children: " + _children.Length);*/
        }

        /*private void Update()
        {
            if (fadeIn)
            {
                trigger(true, false);
            }
            else if (fadeOut)
            {
                trigger(false, true);
            }
        }*/

        public void trigger() //(bool fadeIn, bool fadeOut)
        {
            foreach (var child in _children)
            {
                child.startFadingOut();
            }
        }
    }
}