using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Portal : MonoBehaviour 
{
    public GameObject portalToTP;
    private GameObject player;
    private GameObject _interactText;
    public float fadingTime;
    public GameObject interactTextPrefab;
    private bool _canInteract;
    public CanvasGroup _fadePanel;
    
    void Start()
    {
        player = GlobalController.Instance.player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
            showInteractText();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
            hideInteractText();
        }
    }

    private void Update()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(fadeOut());
        }
    }
        
    private void showInteractText()
    {
        if (_interactText == null)
        {
            _interactText = Instantiate(interactTextPrefab, transform.position + new Vector3(0, 1.5f, 0),
                Quaternion.identity);
            _interactText.GetComponentInChildren<Text>().text = $"Press X to Interact";
        }
    
        _interactText.GetComponentInChildren<Text>().text = $"Press X to Interact";
    }

    /// <summary> The hideInteractText function destroys the interact text object.</summary>
    /// <returns> Nothing.</returns>
    private void hideInteractText()
    {
        if (_interactText != null)
        {
            Destroy(_interactText);
            _interactText = null;
        }
    }

    private IEnumerator fadeOut()
    {
        
        _fadePanel.gameObject.SetActive(true);
        hideInteractText();
        
        CanvasGroup canvasGroup = _fadePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        Debug.Log("Fading out");
        // Fade out over 1 second
        float elapsedTime = 0f;
        while (elapsedTime < fadingTime)
        {
            Debug.Log("FaDING STILL");
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = new Vector2(portalToTP.transform.position.x, portalToTP.transform.position.y);

        Debug.Log("Should've faded by now lol");
        // Set alpha to 1f
        canvasGroup.alpha = 1f;
        //yield return new WaitForSeconds(1);
    }

    internal IEnumerator fadeIn()
    {
        // Set alpha to 1f
        CanvasGroup canvasGroup = _fadePanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        
        Debug.Log("Fading In");

        // Fade in over 1 second
        float elapsedTime = 0f;
        while (elapsedTime < fadingTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 1f);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set alpha to 0f
        canvasGroup.alpha = 0f;
    }
}