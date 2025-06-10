using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadingScreenManager.Instance.LoadSceneWithLoading("Tunel_Inside");
            Debug.Log("Cambiando a siguiente nivel");
        }
    }
}
