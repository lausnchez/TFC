using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    [System.Serializable]
    public class CardEffect
    {
        public int id;
        public string name;
        public string description;
        public int active_turns;
        public int value;
    }

    [System.Serializable]
    public class CardDataAPI
    {
        public int id;
        public string name;
        public string description;
        public int cost;
        public int currency_cost;
        public string image;
        public string target;
        public float rarity;
        public List<CardEffect> effects;
    }


    public static class JsonUtilityWrapper
    {
        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> items;
        }

        public static List<T> FromJson<T>(string json)
        {
            // Para que JsonUtility pueda parsear un array JSON, lo envolvemos en un objeto con "items"
            string newJson = "{\"items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.items;
        }
    }


}
