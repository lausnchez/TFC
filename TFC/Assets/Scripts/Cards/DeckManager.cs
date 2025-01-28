using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TFC;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private int currentIndex = 0;
    private void Start()
    {
        // Load all card assets from the Resources folder
        Card[] cards = Resources.LoadAll<Card>("Cards");
        // Add the loaded cards to the all cards list
        allCards.AddRange(cards);

        HandManager hand = FindObjectOfType<HandManager>(); // Encuentra un objeto de tipo HandManager
        for (int i = 0; i < 6; i++)
        {
            DrawCard(hand);
        }
    }
    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
            return;

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }
}
