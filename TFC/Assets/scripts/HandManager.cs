using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform spawnPoint;

    private List<GameObject> handCards = new();
    private HandView handView;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        DrawCard();
    //}

    //public void DrawCard()
    //{
    //    Debug.Log("HandManager DRAW");
    //    if (handCards.Count < maxHandSize)
    //    {
    //        if (cardPrefab != null && spawnPoint != null)
    //        {
    //            GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
    //            handCards.Add(newCard);
    //            Debug.Log("Card drawn successfully.");
    //            Debug.Log($"Card Position: {newCard.transform.position}, Scale: {newCard.transform.localScale}");
    //            StartCoroutine(handView.UpdateCardPositions(0.15f));
    //            //UpdateCardPositions();
    //        }
    //        else
    //            Debug.LogError("cardPrefab or spawnPoint is not assigned.");
    //    }
    //    else
    //        Debug.Log("Max hand size reached. Cannot draw more cards.");
    //}

    //public void RemoveCard(GameObject card)
    //{
    //    Debug.Log("HandManager REMOVE");
    //    if (handCards.Contains(card))
    //    {
    //        handCards.Remove(card);
    //        StartCoroutine(handView.UpdateCardPositions(0.15f));
    //    }
    //}
}
