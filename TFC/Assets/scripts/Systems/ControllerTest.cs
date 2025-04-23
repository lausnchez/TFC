using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    [SerializeField] private HandView handView;
    private List<CardView> cards;
    private int currentIndex = 0;

    private float inputCooldown = 0.25f; // Tiempo entre inputs
    private float lastInputTime;

    private CardView selectedCard;

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
        bool dpadLeft = Input.GetKeyDown(KeyCode.JoystickButton7); // D-Pad Left (dependiendo del mando puede variar)
        bool dpadRight = Input.GetKeyDown(KeyCode.JoystickButton8); // D-Pad Right

        if ((horizontal > 0.5f || dpadRight || Input.GetKeyDown(KeyCode.RightArrow)) && Time.time - lastInputTime > inputCooldown)
        {
            MoveSelection(1);
            lastInputTime = Time.time;
        }
        else if ((horizontal < -0.5f || dpadLeft || Input.GetKeyDown(KeyCode.LeftArrow)) && Time.time - lastInputTime > inputCooldown)
        {
            MoveSelection(-1);
            lastInputTime = Time.time;
        }

        // Si se presiona el botón de acción (A en el mando)
        if (Input.GetButtonDown("Fire1")) // Puede ser "Fire1" o el botón A del mando de Xbox
        {
            MoveCardToEnemy(selectedCard);
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

    // Mover la carta hacia el enemigo
    private void MoveCardToEnemy(CardView card)
    {
        if (card == null)
        {
            Debug.LogError("La carta no está inicializada.");
            return; // Salir de la función si la carta no está inicializada
        }

        EnemyController enemy = FindClosestEnemy();
        if (enemy == null)
        {
            Debug.LogError("No se ha encontrado un enemigo.");
            return; // Salir de la función si no se encuentra un enemigo
        }

        // Mueve la carta hacia el enemigo
        card.transform.position = Vector3.MoveTowards(card.transform.position, enemy.transform.position, Time.deltaTime * 5f); // Ajusta la velocidad según sea necesario

        // Detectar si ha llegado al enemigo
        if (Vector3.Distance(card.transform.position, enemy.transform.position) < 0.1f)
        {
            // Hacer daño al enemigo
            enemy.TakeDamage(10); // Aplica 10 de daño
            Debug.Log("Se ha hecho daño al enemigo.");
        }
    }

    private EnemyController FindClosestEnemy()
    {
        // Encuentra el enemigo más cercano en la escena
        EnemyController[] enemigos = FindObjectsOfType<EnemyController>();
        if (enemigos.Length == 0)
        {
            return null; // No se encontró ningún enemigo
        }

        EnemyController closestEnemy = enemigos[0];
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

        foreach (EnemyController enemy in enemigos)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
