using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Utilities.Test_Scripts;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    public LevelManager lm;
    public GameObject interactTextPrefab;

    private GameObject _interactText;
    private bool _canInteract;
    private KeyBindingManager _kbm;
    private KeyCode _interactKey;
    private Collider2D _collider;

    private void Start()
    {
        _collider = gameObject.GetComponent<Collider2D>();
        _interactKey = _kbm.interact;
    }

    /// <summary> The OnTriggerEnter2D function is called when the Collider2D other enters the trigger.</summary>    
    ///
    /// <param name="Collider2D collision"> /// the collider2d that is colliding with this object.
    /// </param>
    ///
    /// <returns> A void.</returns>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
            showInteractText();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject == GlobalController.Instance.player)
        {
            _canInteract = true;
            showInteractText();
        }
    }

    /// <summary> The OnTriggerExit2D function is called when the Collider2D other has stopped touching the trigger.</summary>    
    ///
    /// <param name="Collider2D collision"> /// the collider2d that is colliding with the trigger.
    /// </param>
    ///
    /// <returns> Nothing.</returns>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
            hideInteractText();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject == GlobalController.Instance.player)
        {
            _canInteract = false;
            hideInteractText();
        }
    }

    /// <summary> The Update function is called once per frame. It checks if the player can interact with the object,
    /// and if so, it switches layers.</summary>    
    ///
    ///
    /// <returns> Nothing.</returns>
    private void Update()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.I))
        {
            lm.OnTriggerEnterWithDoor();
        }

        if (_canInteract && lm.loadingOperation.isDone)
        {
            lm.player.transform.position = GameObject.FindWithTag("Door").transform.position;
            lm.OnTriggerExitWithDoor();
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GlobalController.Instance.player)
        {
            lm.OnTriggerExitWithDoor();
        }
    }*/

    /// <summary> The showInteractText function instantiates the interactTextPrefab at the player's position,    
    /// plus an offset of (0, 1.5f, 0) to make it appear above their head.</summary>
    ///
    ///
    /// <returns> Nothing.</returns>
    private void showInteractText()
    {
        if (_interactText == null)
        {
            _interactText = Instantiate(interactTextPrefab, transform.position + new Vector3(0, 1.5f, 0),
                Quaternion.identity);
            _interactText.GetComponentInChildren<Text>().text = $"Press {_interactKey} to Interact";
        }
        
        _interactText.GetComponentInChildren<Text>().text = $"Press {_interactKey} to Interact";
    }

    /// <summary> The hideInteractText function destroys the interact text object.</summary>    
    ///
    ///
    /// <returns> Nothing.</returns>
    private void hideInteractText()
    {
        if (_interactText != null)
        {
            Destroy(_interactText);
            _interactText = null;
        }
    }
}

