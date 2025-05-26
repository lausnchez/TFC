using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;

public class CreateCardView : Singleton<CreateCardView>
{
    [SerializeField] private CardView cardPrefab;

    private List<Cards.CardDataAPI> apiCardDataList = new List<Cards.CardDataAPI>();

    private void Awake()
    {
        base.Awake();

        // Cambia la URL por la tuya
        StartCoroutine(LoadCardsFromAPI("https://tfgvideojuego.lausnchez.es/api/card"));
    }

    private IEnumerator LoadCardsFromAPI(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))//creamos una peticion a la api
        {
            yield return www.SendWebRequest();//esperamos que termine la peticion

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error cargando API: " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;//se obtiene el contenido textual de la api,downloadHandler.text es lo que devuelve el server en este caso el array de cartas
                apiCardDataList = Cards.JsonUtilityWrapper.FromJson<Cards.CardDataAPI>(json);
                Debug.Log($"Cartas cargadas: {apiCardDataList.Count}");
            }
        }
    }

    public CardView CreatorCardView(Vector3 position, Quaternion rotation)
    {
        if (apiCardDataList == null || apiCardDataList.Count == 0)
        {
            Debug.LogWarning("No hay cartas cargadas para mostrar.");
            return null;
        }

        int randomIndex = Random.Range(0, apiCardDataList.Count);
        Cards.CardDataAPI apiCard = apiCardDataList[randomIndex];

        CardView cardView = Instantiate(cardPrefab, position, rotation);
        cardView.gameObject.tag = "Card";
        cardView.transform.localScale = Vector3.zero;

        HoverTest hover = cardView.GetComponent<HoverTest>();
        if (hover == null)
            hover = cardView.gameObject.AddComponent<HoverTest>();

        cardView.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
        {
            hover.originalScale = cardView.transform.localScale;
        });

        // Aquí debes adaptar SetCardData para aceptar Cards.CardDataAPI o hacer otro método
        cardView.SetCardData(apiCard);

        Transform borderTransform = cardView.transform.Find("Wrapper/BorderObject");
        if (borderTransform != null)
        {
            hover.borderObject = borderTransform.gameObject;
            hover.borderObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró el objeto 'BorderObject' dentro de Wrapper.");
        }

        return cardView;
    }
}
