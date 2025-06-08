using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private HandView handView;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (!handView.CanAddCard())
            {
                Debug.Log("No se puede robar carta: mano llena.");
                // Aquí podrías poner un popup visual si quieres
                return;
            }

            CardView cardView = CreateCardView.Instance.CreatorCardView(transform.position, Quaternion.identity);

            if (cardView != null)
            {
                StartCoroutine(handView.AddCard(cardView));
            }
        }
    }
}
