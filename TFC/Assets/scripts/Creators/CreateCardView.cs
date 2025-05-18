using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CreateCardView : Singleton<CreateCardView>
{
    [SerializeField] private CardView cardPrefab;

    [Header("Cartas disponibles")]
    [SerializeField] private Sprite fireCard;
    [SerializeField] private Sprite iceCard;
    [SerializeField] private Sprite lightning;
    [SerializeField] private Sprite poison;

    private List<CardData> cardDataList;

    private void Awake()
    {
        base.Awake();
        cardDataList = new List<CardData>()
        {
            new CardData() { title = "Fuego", description = "Quema al enemigo", image = fireCard },
            new CardData() { title = "Hielo", description = "Congela al enemigo", image = iceCard },
            new CardData() { title = "Rayo", description = "Electrocuta al enemigo", image = lightning },
            new CardData() { title = "Veneno", description = "Envenena al enemigo", image = poison },
        };
    }

    public CardView CreatorCardView(Vector3 position, Quaternion rotation)
    {
        int randomIndex = Random.Range(0, cardDataList.Count);
        CardData randomData = cardDataList[randomIndex];


        CardView cardView = Instantiate(cardPrefab, position, rotation);
        cardView.gameObject.tag = "Card";
        cardView.transform.localScale = Vector3.zero;

        HoverTest hover = cardView.GetComponent<HoverTest>();
        if (hover == null)
            hover = cardView.gameObject.AddComponent<HoverTest>();

        // Escalado inicial con DOTween y guardamos originalScale al finalizar
        cardView.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
        {
            hover.originalScale = cardView.transform.localScale;
        });

        cardView.SetCardData(randomData);

        // Buscar el borde dentro del wrapper
        Transform borderTransform = cardView.transform.Find("Wrapper/BorderObject");
        if (borderTransform != null)
        {
            hover.borderObject = borderTransform.gameObject;
            hover.borderObject.SetActive(false); // lo ocultamos por defecto
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró el objeto 'BorderObject' dentro de Wrapper.");
        }

        return cardView;
    }
}
