using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TFC;
using System;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    // Propiedades del inspector
    public GameObject cardPrefab;   // Assign card prefab in inspector
    public Transform handTransform; // Root of the hand position

    public int maxHandSize = 5;
    public float fanSpread = 7.4f;
    public float cardSpacing = -175f;
    public float verticalSpacing = 65f;

    public List<GameObject> cardsInHand = new List<GameObject>();   // Lista de cartas en mano

    void Start()
    {

    }

    public void AddCardToHand(Card cardData)
    {
        if (cardsInHand.Count < maxHandSize)
        {
            // Instancia la carta con las propiedades de arriba
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            // Set the card data of the instanciated card
            newCard.GetComponent<CardDisplay>().cardData = cardData;
            // Actualiza la mano
            UpdateHandVisuals();
        }
    }

    private void Update()
    {
       UpdateHandVisuals();
        /*
         8
        232.3
        72.6
         */
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            // Colocar las cartas
            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); // Normalize card position between 1 and -1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            // Set card positions
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
