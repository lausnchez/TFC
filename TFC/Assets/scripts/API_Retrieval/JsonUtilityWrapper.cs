using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtilityWrapper : MonoBehaviour
{
    // Clase para convertir JSON a objetos de Unity usando JsonUtility
    private class Wrapper<T>
    {
        public List<T> items;
    }

    // Convierte el JSON a una lista de objetos
    public static List<T> FromJson<T>(string apiType,string json)
    {
        // Para que JsonUtility pueda parsear un array JSON, lo envolvemos en un objeto con apiType
        // apiType = enemy, card, item, effect... Está todo en la doc: https://tfgvideojuego.lausnchez.es/
        string newJson = "{\""+ apiType +"\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }
}
