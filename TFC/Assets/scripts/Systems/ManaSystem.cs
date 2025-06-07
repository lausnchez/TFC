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

    // Pone el mana al m�ximo
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

    // Inicializa el sistema de mana con un valor inicial
    public void InitializeManaSystem(int manaInitial)
    {
        MaxMana = manaInitial;
        ResetMana();
    }

    // Comprueba si hay suficiente mana disponible para una acci�n
    public bool isManaAvailable(int manaCost)
    {
        return manaCost <= CurrentMana;
    }

    // Intenta usar mana, devuelve true si se pudo usar, false si no hay suficiente mana
    public bool UseMana(int manaCost)
    {
        if (isManaAvailable(manaCost))
        {
            RemoveMana(manaCost);
            return true;
        }
        else return false;
    }

    // Agregar mana, en caso de que se exceda el m�ximo, se ajusta al m�ximo permitido
    public void AddMana(int manaAmount)
    {
        CurrentMana += manaAmount;
        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana; // Asegura que no exceda el m�ximo
        }
        UpdateManaDisplay();
    }

    // Elimina mana, en caso de que se vuelva negativo, se ajusta a cero
    public void RemoveMana(int manaAmount)
    {
        CurrentMana -= manaAmount;
        if (CurrentMana < 0)
        {
            CurrentMana = 0; // Asegura que no sea negativo
        }
        UpdateManaDisplay();
    }

    // Agrega man� m�ximo
    public void addToManaMax(int extraMana)
    {
        if(MaxMana < 100)
            MaxMana += extraMana;
        else
            MaxMana = 99; // Limita el m�ximo a 99
    }
}
