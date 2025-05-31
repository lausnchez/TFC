using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyLifeBar : MonoBehaviour
{
    public Slider slider;
    public Color low_color;
    public Color high_color;
    public Vector3 Offset = new Vector3(0,0.4f,0);
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enemyNameText;

    public void SetHealth(int health, int maxHealth, UnityEngine.Transform enemyTransform, float spriteHeight)
    {
        slider.gameObject.SetActive(true);
        slider.value = (float)health / maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low_color, high_color, (float)health / maxHealth);
        healthText.text = health.ToString("D2");
        
        Vector3 posicionBase = enemyTransform.position + new Vector3(0, spriteHeight + Offset.y, 0);
        transform.position = posicionBase;
        healthText.transform.position = posicionBase + new Vector3(1.60f,0,0);
        enemyNameText.transform.position = posicionBase + new Vector3(0,0.35f,0);
    }
}
