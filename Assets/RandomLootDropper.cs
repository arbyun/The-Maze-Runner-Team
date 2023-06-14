using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using TMPro;

public class RandomLootDropper : MonoBehaviour
{
    private InventoryInstance _inventory;
    public List<Item> lootTable;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI resultText;

    private bool isPlayerInside;

    private void Awake() 
    {
        _inventory = ItemManager.Instance.inventory;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            interactionText.text = "Press X to Open Chest";
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            interactionText.gameObject.SetActive(false);
            resultText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.X))
        {
            interactionText.gameObject.SetActive(false);
            resultText.text = "You got " + GetRandomReward();
            resultText.gameObject.SetActive(true);
            StartCoroutine(FadeOut(resultText));
            StartCoroutine(FadeOut(gameObject.GetComponent<SpriteRenderer>()));
        }
    }

    private string GetRandomReward()
    {
        int randomIndex = UnityEngine.Random.Range(0, lootTable.Count);
        _inventory.AddItem(lootTable[randomIndex]);
        
        return lootTable[randomIndex].itemName;
    }

    private IEnumerator FadeOut(TextMeshProUGUI text)
    {
        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float duration = 1.0f; // Adjust this value to control the fade-out duration

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            text.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            yield return null;
        }

        text.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float duration = 1.0f; // Adjust this value to control the fade-out duration

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
}