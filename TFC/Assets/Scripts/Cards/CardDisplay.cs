using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TFC;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image[] typeImages;
    public Image illustration;

    // Colores
    private Color[] cardColors =
    {
        new Color(84/255f, 11/255f, 0/255f),  // Fire
        new Color(66/255f, 35/255f, 16/255f),    // Earth    
        new Color(0/255f, 65/255f, 84/255f), // Water
        new Color(33/255f, 84/255f, 27/255f),   // Air       
    };

    private Color[] typeColors =
    {
        new Color(232/255f, 56/255f, 2/255f),  // Fire
        new Color(179/255f, 122/255f, 87/255f), // Earth 
        new Color(122/255f, 217/255f, 255/255f), // Water
        new Color(140/255f, 255/255f, 122/255f)   // Air
    };

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        // Update the main card image color based on the first card type
        cardImage.color = cardColors[(int)cardData.cardType[0]];
        illustration.color = typeColors[(int)cardData.cardType[0]];
        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        damageText.text = $"{cardData.damageMin}-{cardData.damageMax}";

        // Update type images
        for (int i = 0; i < typeImages.Length; i++)
        {
            // Comprobamos cuantos tipos hay y los hace visibles
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else
            {
                typeImages[i].gameObject.SetActive(false);

            }
        }
    }
}
