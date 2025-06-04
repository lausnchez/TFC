using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaSystem : MonoBehaviour
{
    public static ManaSystem Instance { get; private set; } // Singleton instance
    [SerializeField] private TextMeshProUGUI manaText;

    private int MaxMana;
    private int CurrentMana;

    private void Awake()
    {
        // Asignar la instancia del singleton
        if (Instance == null)
            Instance = this;
        ResetMana();
    }

    // Pone el mana al máximo
    public void ResetMana()
    {
        CurrentMana = MaxMana;
        UpdateManaDisplay();
    }

    // Actualiza la cifra de manaText
    public void UpdateManaDisplay()
    {
        if (manaText != null)
        {
            manaText.text = CurrentMana.ToString();
        }
        else
        {
            Debug.LogWarning("Mana text UI element is not assigned.");
        }
    }

    public void InitializeManaSystem(int manaInitial)
    {
        MaxMana = manaInitial;
        ResetMana();

    }

}
