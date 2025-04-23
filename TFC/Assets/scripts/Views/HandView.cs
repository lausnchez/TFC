using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{

    [SerializeField] private SplineContainer splineContainer;

    private readonly List<CardView> cards = new();
    public List<CardView> GetCards()
    {
        return cards;
    }

    public IEnumerator AddCard(CardView cardview)
    {
        cards.Add(cardview);
        yield return UpdateCardPositions(0.15f);
        Debug.Log("No sale ADDCARD");


    }

    public IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0)
        {
            yield break;
        }
        float cardSpacing = 1f / 10f;
        float firstCardPoaition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPoaition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        Debug.Log("No sale UPDATECARD");
        yield return new WaitForSeconds(duration);

    }

}
