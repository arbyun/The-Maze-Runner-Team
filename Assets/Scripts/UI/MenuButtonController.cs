using UnityEngine;

namespace UI
{
    public class MenuButtonController : MonoBehaviour
    {
        public AudioSource audioSource;
        public int index;
        [SerializeField] private bool keyDown;
        [SerializeField] private int maxIndex;

        private void Start () 
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary> Handles the logic for changing the index of the menu item that is
        /// currently selected.</summary>
        internal void Update () 
        {
            if (Input.GetAxis ("Vertical") != 0)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis ("Vertical") < 0) 
                    {
                        if (index < maxIndex)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    } 
                    else if(Input.GetAxis ("Vertical") > 0)
                    {
                        if (index > 0)
                        {
                            index --; 
                        }
                        else
                        {
                            index = maxIndex;
                        }
                    }
                    
                    keyDown = true;
                }
            }
            else
            {
                keyDown = false;
            }
        }
        
    }
}