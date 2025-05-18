using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private GameObject wrapper;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Para que no sea afectado por la gravedad.
        }
    }

    public void SetCardData(CardData data)
    {
        title.text = data.title;
        description.text = data.description;
        image.sprite = data.image;
    }

    // Detectar colisiones con enemigos
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemigo"))
        {
            EnemyController enemyHealth = other.GetComponent<EnemyController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // Aplica 10 de daño
                Debug.Log("¡La carta golpeó al enemigo y le hizo daño!");
            }
        }
    }
}
