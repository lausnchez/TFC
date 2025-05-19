using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    [SerializeField] private HandView handView;
    private List<CardView> cards;
    private int currentIndex = 0;

    private float inputCooldown = 0.25f; // Tiempo entre inputs
    private float lastInputTime;

    private CardView selectedCard;
    private Controller controller;

    void Awake()
    {
        controller = new();
    }
    void Start()
    {

        cards = handView.GetCards();
        if (cards.Count > 0)
        {
            HighlightCard(currentIndex);
            selectedCard = cards[currentIndex]; // Inicia con la carta seleccionada
        }
    }


    void Update()
    {
        if (cards == null || cards.Count == 0) return;

        // Entrada para seleccionar carta
        float horizontal = Input.GetAxisRaw("Horizontal");
        float horizontal1 = Input.GetAxisRaw("dpadHorizontal");//este es para el dpad
        if ((horizontal > 0.5f || horizontal1 > 0.5f || Input.GetKeyDown(KeyCode.RightArrow)) && Time.time - lastInputTime > inputCooldown)
        {
            MoveSelection(1);
            lastInputTime = Time.time;
        }
        else if ((horizontal < -0.5f || horizontal1 < -0.5f || Input.GetKeyDown(KeyCode.LeftArrow)) && Time.time - lastInputTime > inputCooldown)
        {
            MoveSelection(-1);
            lastInputTime = Time.time;
        }



    }

    private void MoveSelection(int direction)
    {
        UnhighlightCard(currentIndex);

        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = cards.Count - 1;
        else if (currentIndex >= cards.Count)
            currentIndex = 0;

        HighlightCard(currentIndex);
    }

    private void HighlightCard(int index)
    {
        HoverTest hover = cards[index].GetComponent<HoverTest>();
        if (hover != null)
        {
            hover.ActivateHover();
        }
    }

    private void UnhighlightCard(int index)
    {
        HoverTest hover = cards[index].GetComponent<HoverTest>();
        if (hover != null)
        {
            hover.DeactivateHover();
        }
    }


}
