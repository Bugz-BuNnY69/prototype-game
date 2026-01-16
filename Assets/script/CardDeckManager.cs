using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject deckSelectionPanel;   // Panel with 3 buttons
    public Transform gridParent;            // GridLayoutGroup parent
    public Text scoreText;                  // Score display
    public Text wrongTriesText;             // Remaining wrong tries
    public GameObject resultPanel;          // Congrats / Lost panel
    public Text resultText;                 // Text in result panel
    public Button resultButton;             // Button to restart

    [Header("Card Setup")]
    public GameObject cardPrefab;           // Card prefab (with card script)

    private List<GameObject> spawnedCards = new List<GameObject>();

    private List<card> flippedCards = new List<card>();
    private int score = 0;
    private int wrongTriesLeft = 3;

    // ---------- BUTTON EVENTS ----------
    public void Create6CardDeck() { CreateDeck(6); }
    public void Create8CardDeck() { CreateDeck(8); }
    public void Create12CardDeck() { CreateDeck(12); }

    // ---------- CORE LOGIC ----------
    void CreateDeck(int totalCards)
    {
        ClearOldCards();

        // Reset game state
        flippedCards.Clear();
        score = 0;
        wrongTriesLeft = 3;
        UpdateUI();

        // Hide selection panel
        if (deckSelectionPanel != null)
            deckSelectionPanel.SetActive(false);

        // Hide result panel
        if (resultPanel != null)
            resultPanel.SetActive(false);

        int pairCount = totalCards / 2;

        // Create paired sprite indexes
        List<int> spriteIndexes = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            spriteIndexes.Add(i);
            spriteIndexes.Add(i);
        }

        // Shuffle pairs
        Shuffle(spriteIndexes);

        // Spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridParent);
            spawnedCards.Add(cardObj);

            card cardScript = cardObj.GetComponent<card>();
            if (cardScript != null)
            {
                cardScript.SetCardImage(spriteIndexes[i]);
                cardScript.deckManager = this; // Give reference to manager
            }
        }
    }

    // Called by card when it is flipped
    public void CardFlipped(card flippedCard)
    {
        if (!flippedCards.Contains(flippedCard))
            flippedCards.Add(flippedCard);

        if (flippedCards.Count == 2)
            StartCoroutine(CheckPair());
    }

    IEnumerator CheckPair()
    {
        // Wait a bit so player can see second card
        yield return new WaitForSeconds(0.5f);

        card card1 = flippedCards[0];
        card card2 = flippedCards[1];

        if (card1.spriteIndex == card2.spriteIndex)
        {
            // Match: remove cards
            card1.gameObject.SetActive(false); // hide for now (particle later)
            card2.gameObject.SetActive(false);

            score += 5;
        }
        else
        {
            // Not matched: flip back
            card1.FlipBack();
            card2.FlipBack();

            wrongTriesLeft--;
            score -= 10;
        }

        flippedCards.Clear();
        UpdateUI();

        // Check for win/loss
        CheckGameEnd();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (wrongTriesText != null)
            wrongTriesText.text = "Wrong tries left: " + wrongTriesLeft;
    }

    void CheckGameEnd()
    {
        // Win: all cards inactive
        bool allCleared = true;
        foreach (var c in spawnedCards)
        {
            if (c.activeSelf)
            {
                allCleared = false;
                break;
            }
        }

        if (allCleared)
        {
            ShowResult("Congrats! You Won!");
        }
        else if (wrongTriesLeft <= 0)
        {
            // Lose: remove all remaining cards
            foreach (var c in spawnedCards)
                c.SetActive(false);

            ShowResult("You Lost!");
        }
    }

    void ShowResult(string message)
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
            resultText.text = message;

        if (resultButton != null)
        {
            resultButton.onClick.RemoveAllListeners();
            resultButton.onClick.AddListener(() =>
            {
                deckSelectionPanel.SetActive(true);
                resultPanel.SetActive(false);
                ClearOldCards();
            });
        }
    }

    // ---------- UTILITIES ----------
    void ClearOldCards()
    {
        foreach (GameObject card in spawnedCards)
            Destroy(card);

        spawnedCards.Clear();
        flippedCards.Clear();
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
