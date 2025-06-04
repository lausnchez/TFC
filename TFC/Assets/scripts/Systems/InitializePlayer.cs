using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class InitializePlayer : MonoBehaviour
{
    private Characters.CharacterData characterData;
    public Transform player;
    public TextMeshProUGUI playerLifeCounter;
    //public TextMeshProUGUI playerStaminaCounter;

    void Awake()
    {
        player = GameAssets.i.Player;
        StartCreateCharacter("https://tfgvideojuego.lausnchez.es/api/character", 1);
    }

    public void StartCreateCharacter(string url, int idCharacter)
    {
        StartCoroutine(CreateCharacterCoroutine(url, idCharacter));
    }

    private IEnumerator CreateCharacterCoroutine(string url, int idCharacter)
    {
        //manaSystem = GameAssets.i.ManaDisplay.GetComponent<ManaSystem>();
        string urlWithID = url + "/" + idCharacter;
        using (UnityWebRequest www = UnityWebRequest.Get(urlWithID))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error cargando API: " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log("JSON recibido: " + json);
                characterData = JsonUtility.FromJson<Characters.CharacterData>(json);
                // Inicializar el jugador con los datos del personaje
                var staminaControler = player.GetComponent<PlayerStaminaController>();
                if (staminaControler != null)
                {
                    staminaControler.InicializarJugador(characterData.health, characterData.stamina);
                }
                playerLifeCounter.text = characterData.health.ToString(); // Inicializa el contador de vida del jugador
                ManaSystem.Instance.InitializeManaSystem(characterData.stamina); // Inicializa el sistema de mana
            }
        }
    }
}
