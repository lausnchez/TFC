using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEffect : MonoBehaviour
{
    public string name;
    public int turnos_restantes;
    public int value;

    public ActiveEffect(string name, int turnos_restantes, int value)
    {
        this.name = name;
        this.turnos_restantes = turnos_restantes;
        this.value = value;
    }
}
