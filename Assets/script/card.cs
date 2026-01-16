using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class card : MonoBehaviour, IPointerClickHandler
{
    [Header("Card Images")]
    public Image cardFront;

    [Header("Available Sprites")]
    public List<Sprite> cardSprites = new List<Sprite>();

    [Header("Flip Settings")]
    public float flipDuration = 0.2f;

    [HideInInspector]
    public int spriteIndex;

    [HideInInspector]
    public CardDeckManager deckManager;

    bool isFlipping = false;

    void Start()
    {
        if (cardFront != null)
            cardFront.gameObject.SetActive(false);
    }

    public void SetCardImage(int index)
    {
        spriteIndex = index;
        cardFront.sprite = cardSprites[index];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFlipping && !cardFront.gameObject.activeSelf && deckManager != null)
        {
            StartCoroutine(FlipAnimation(true));
            deckManager.CardFlipped(this);
        }
    }

    // Flip card
    IEnumerator FlipAnimation(bool turnOn)
    {
        isFlipping = true;

        float time = 0f;
        Vector3 startScale = transform.localScale;

        // Shrink
        while (time < flipDuration)
        {
            time += Time.deltaTime;
            float xScale = Mathf.Lerp(1f, 0f, time / flipDuration);
            transform.localScale = new Vector3(xScale, startScale.y, startScale.z);
            yield return null;
        }

        // Turn on/off front
        if (cardFront != null)
            cardFront.gameObject.SetActive(turnOn);

        time = 0f;

        // Expand
        while (time < flipDuration)
        {
            time += Time.deltaTime;
            float xScale = Mathf.Lerp(0f, 1f, time / flipDuration);
            transform.localScale = new Vector3(xScale, startScale.y, startScale.z);
            yield return null;
        }

        transform.localScale = startScale;
        isFlipping = false;
    }

    // Flip back (called by deck manager)
    public void FlipBack()
    {
        StartCoroutine(FlipAnimation(false));
    }
}
