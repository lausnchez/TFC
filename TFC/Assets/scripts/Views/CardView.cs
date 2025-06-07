using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private GameObject wrapper;
    public int cardDamage { get; set; }
    public int cardMana { get; set; }

    private Cards.CardDataAPI cardData;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Para que no sea afectado por la gravedad.
        }
    }

    public void SetCardData(Cards.CardDataAPI data)
    {
        cardData = data;

        title.text = data.name;
        description.text = data.description;
        // image.sprite = data.image;
        mana.text = data.cost.ToString();
        cardDamage = calculateCardDamage();
        cardMana = data.cost;
        Debug.Log($"Card Damage Calculated: {cardDamage}");
    }

    // Detectar colisiones con enemigos
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemigo"))
        {
            EnemyController enemy_controller = other.GetComponent<EnemyController>();
            if (enemy_controller != null)
            {
                enemy_controller.TakeDamage(cardDamage);
                
                //enemy_controller.TakeDamage(totalDamage); // Aplica 10 de daño
                //enemy_controller.ApplyEffects(cardData.effects); // Aplica los efectos de la carta
                Debug.Log("¡La carta golpeó al enemigo y le hizo daño!");
            }
        }
    }

    private int calculateCardDamage()
    {
        int totalDamage = 0;
        foreach (Cards.CardEffect effect in cardData.effects)
        {
            totalDamage += effect.value;
        }
        return totalDamage;
    }
}
