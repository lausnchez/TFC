using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChangeScene : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Guardar la posición actual del jugador
            Vector3 playerPos = collision.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", playerPos.x - 0.8f);
            PlayerPrefs.SetFloat("PlayerPosY", playerPos.y - 0.8f);
            PlayerPrefs.Save();

            // Cambiar de escena
            LoadingScreenManager.Instance.LoadSceneWithLoading("Shop");
            Debug.Log("Cambiando a Shop...");
        }
    }
}
