using System.Collections.Generic;
using UnityEngine;

public class CardDeckManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject deckSelectionPanel;   // Panel with 3 buttons
    public Transform gridParent;            // GridLayoutGroup parent

    [Header("Card Setup")]
    public GameObject cardPrefab;            // Card prefab (with Card script)

    // Store spawned cards so we can clear them
    private List<GameObject> spawnedCards = new List<GameObject>();

    // ---------------- BUTTON EVENTS ----------------

    public void Create6CardDeck()
    {
        CreateDeck(6);
    }

    public void Create8CardDeck()
    {
        CreateDeck(8);
    }

    public void Create12CardDeck()
    {
        CreateDeck(12);
    }

    // ---------------- CORE LOGIC ----------------

    void CreateDeck(int totalCards)
    {
        ClearOldCards();

        // Hide selection panel
        if (deckSelectionPanel != null)
            deckSelectionPanel.SetActive(false);

        int pairCount = totalCards / 2;

        // Create pair indices (0,0,1,1,2,2...)
        List<int> spriteIndices = new List<int>();

        for (int i = 0; i < pairCount; i++)
        {
            spriteIndices.Add(i);
            spriteIndices.Add(i);
        }

        // Shuffle indices
        Shuffle(spriteIndices);

        // Spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridParent);
            spawnedCards.Add(cardObj);

            Card card = cardObj.GetComponent<Card>();

            if (card != null)
            {
                card.SetCardImage(spriteIndices[i]);
            }
        }
    }

    // ---------------- UTILITIES ----------------

    void ClearOldCards()
    {
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            Destroy(spawnedCards[i]);
        }
        spawnedCards.Clear();
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
