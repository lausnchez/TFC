using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private Collider2D col;

    private Vector3 startDragPosition;

    private bool isTargeted;
    int damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();

    }

    private void OnMouseDown()
    {
        startDragPosition = transform.position;
        isTargeted = false;
        transform.position = GetMousePositionInWorldSpace();//pilla la posicion del raton
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePositionInWorldSpace();
    }

    private void OnMouseUp()
    {
        if (PlayerStaminaController.Instance != null && PlayerStaminaController.Instance.CanUseCard())
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("enemigo"))
                {
                    EnemyController enemy = hit.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage); // Inflige da�o al enemigo
                        PlayerStaminaController.Instance.UseCard(); // Reduce stamina
                        HandManager handManager = FindObjectOfType<HandManager>();
                        if (handManager != null)
                        {
                            handManager.RemoveCard(gameObject);
                        }
                        Destroy(gameObject); // Destruye la carta
                    }
                    return;
                }
            }
        }
        else
        {
            Debug.Log("No puedes usar esta carta. Insuficiente mana.");
        }

        // Si no colisiona con un enemigo, vuelve a la posici�n original
        transform.position = startDragPosition;
    }

    private Vector3 GetMousePositionInWorldSpace()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0f;
        return p;
    }
    // Update is called once per frame
    void Update()
    {

    }

}
