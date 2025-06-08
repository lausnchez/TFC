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

            CardView cardView = CreateCardView.Instance.CreatorCardView(transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));
        }
        

    }
}
