using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public Vector3 Offset;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}

