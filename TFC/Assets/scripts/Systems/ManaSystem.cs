using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    private int MaxMana = 10;
    private int manaCurrent;

    public int GetCurrentMana()
    {
        return manaCurrent;
    }

    private void Start()
    {
        ResetMana();
    }

    /**
     * Resetea el mana al valor inicial
     */
    public void ResetMana()
    {
        manaCurrent = MaxMana;
        UpdateManaText(manaCurrent);
        Debug.Log("Mana reset done");
    }


    /***
     * Cambia el texto del mana en la UI
     */
    public void UpdateManaText(int currentMana)
    {
        if (manaText != null)
            manaText.text = currentMana.ToString();
        else
            Debug.LogWarning("Mana text UI element is not assigned.");
    }

    /**
     * Recibe el mana gastado de la carta y si no hay suficiente mana, no updatea el mana actual
     */
    public bool UseMana(int amount)
    {
        if (amount <= manaCurrent)
        {
            manaCurrent -= amount;
            UpdateManaText(manaCurrent);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough mana to use this amount.");
            return false;
        }
    }

    public void RecuperarMana(int mana)
    {
        if ((manaCurrent + mana) <= MaxMana)
        {
            manaCurrent += mana;   
        }
        else
        {
            manaCurrent = MaxMana;
        }
        UpdateManaText(manaCurrent);
    }

    public bool canUseMana(int amount)
    {
        return amount <= manaCurrent;
    }
}
