using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{

    [SerializeField] private SplineContainer splineContainer;

    private readonly List<CardView> cards = new();
    
    [SerializeField] public int cardMax;
    public int currentCards => cards.Count;

    public List<CardView> GetCards()
    {
        return cards;
    }

    public bool CanAddCard()
    {
        if (cards.Count < cardMax)
            return true;
        else
            return false;
    }

    public IEnumerator AddCard(CardView cardview)
    {
        if (CanAddCard())
        {
            Debug.Log("HandView ADD_CARD");
            cards.Add(cardview);
            yield return UpdateCardPositions(0.15f);
        }
        else
        {
            Debug.LogWarning("Cannot add card, hand is full.");
        }
    }

    public IEnumerator UpdateCardPositions(float duration)
    {
        Debug.Log($"Updating card positions with {cards.Count} cards.");
        if (cards.Count == 0)
        {
            yield break;
        }
        float cardSpacing = 1.5f / 10f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }

        yield return new WaitForSeconds(duration);

    }
}
