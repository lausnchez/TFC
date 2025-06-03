using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Serializable]
    public class EnemyAttack
    {
        public int id;
        public string name;
        public string description;
        public int initial_turns;   // Turnos totales de la habilidad
        public int active_turns;    // Turnos que le quedan a la habilidad
        public int value;
        public EffectPivot pivot;
    }

    [Serializable]
    public class EnemyDataAPI
    {
        public int id;
        public string name;
        public int health;
        public string sprite;
        public float rarity;
        public string type;
        public int floor;
        public List<EnemyAttack> effects;
    }

    [Serializable]
    public class EffectPivot
    {
        public int id_enemy;
        public int id_effect;
    }

    public static class EnemyJsonUtilityWrapper
    {
        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> enemies;
        }

        public static List<T> FromJson<T>(string json)
        {
            // Para que JsonUtility pueda parsear un array JSON, lo envolvemos en un objeto con "items"
            string newJson = "{\"enemies\":" + json + "}";
            Debug.Log("JSON recibido: " + newJson); // Log para depuración
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.enemies;
        }
    }
}
