using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyLifeBar : MonoBehaviour
{
    public Slider slider;
    public Color low_color;
    public Color high_color;
    public Vector3 Offset;
    public TextMeshProUGUI healthText;

    public void SetHealth(int health, int maxHealth)
    {
        slider.gameObject.SetActive(true);
        slider.value = (float)health / maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low_color, high_color, health / maxHealth);
        healthText.text = health.ToString("D2");
    }
}
