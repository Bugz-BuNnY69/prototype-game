using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class card : MonoBehaviour, IPointerClickHandler
{
    [Header("Card Images")]
    public Image cardFront;   // Child image (front)

    [Header("Available Sprites")]
    public List<Sprite> cardSprites = new List<Sprite>();

    [Header("Flip Settings")]
    public float flipDuration = 0.2f;

    [HideInInspector]
    public int spriteIndex;

    bool isFlipping = false;

    void Start()
    {
        // Card starts face down
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
        if (!isFlipping && !cardFront.gameObject.activeSelf)
        {
            StartCoroutine(FlipAnimation());
        }
    }

    IEnumerator FlipAnimation()
    {
        isFlipping = true;

        float time = 0f;
        Vector3 startScale = transform.localScale;

        // Shrink (first half)
        while (time < flipDuration)
        {
            time += Time.deltaTime;
            float xScale = Mathf.Lerp(1f, 0f, time / flipDuration);
            transform.localScale = new Vector3(xScale, startScale.y, startScale.z);
            yield return null;
        }

        // Turn front ON at mid-flip
        cardFront.gameObject.SetActive(true);

        time = 0f;

        // Expand (second half)
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
}
