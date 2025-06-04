using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Characters : MonoBehaviour
{
    [Serializable]
    public class CharacterData
    {
        public int id;
        public string name;
        public int health;
        public int currency;
        public int stamina;
        public int max_items;
        public string sprite;   // En principio no se usa
    }

    public static class CharacterJsonUtilityWrapper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public List<T> characters;
        }

        public static List<T> FromJson<T>(string json)
        {
            // Envolvemos el JSON en una clave "characters" para que Unity pueda deserializarlo
            string newJson = "{\"characters\":" + json + "}";
            Debug.Log("JSON recibido: " + newJson); // Para depuración
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.characters;
        }
    }
}
