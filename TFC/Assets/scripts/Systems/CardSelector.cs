using UnityEngine;
using System.Collections;

public class CardSelector : MonoBehaviour
{
    private CardView selectedCard;
    private Vector3 originalScale;
    private bool cardIsMoving = false;

    void Update()
    {
        if (cardIsMoving) return;

        // --- Seleccionar con mouse o A (xbox) ---
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (selectedCard == null)
                TrySelectCard();
            else
                LaunchCardAtEnemy();
        }

        // --- Deseleccionar con click derecho o B (xbox) ---
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            DeselectCard();
        }
    }

    private void TrySelectCard()
    {
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);

        if (hit.collider != null)
        {
            CardView card = hit.collider.GetComponent<CardView>();
            if (card != null)
            {
                selectedCard = card;
                originalScale = selectedCard.transform.localScale;
                selectedCard.transform.localScale = originalScale * 1.3f;
                Debug.Log("Carta seleccionada: " + card.name);
            }
        }
    }

    private void DeselectCard()
    {
        if (selectedCard == null) return;

        selectedCard.transform.localScale = originalScale;
        Debug.Log("Carta deseleccionada: " + selectedCard.name);
        selectedCard = null;
    }

    private void LaunchCardAtEnemy()
    {
        if (selectedCard == null) return;

        EnemyController target = FindClosestEnemy();
        if (target == null)
        {
            Debug.LogWarning("No se encontró enemigo.");
            return;
        }

        StartCoroutine(MoveCardToTarget(selectedCard, target));
    }

    private IEnumerator MoveCardToTarget(CardView card, EnemyController enemy)
    {
        cardIsMoving = true;

        float speed = 5f;
        while (Vector2.Distance(card.transform.position, enemy.transform.position) > 0.1f)
        {
            card.transform.position = Vector2.MoveTowards(card.transform.position, enemy.transform.position, Time.deltaTime * speed);
            yield return null;
        }

        Debug.Log("Carta alcanzó al enemigo");
        enemy.TakeDamage(10); // Aplica daño aquí directamente
        Destroy(card.gameObject);

        selectedCard = null;
        cardIsMoving = false;
    }

    private EnemyController FindClosestEnemy()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        EnemyController closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}
