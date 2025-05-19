using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightsChangeScene : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadingScreenManager.Instance.LoadSceneWithLoading("FIGHT");
            Debug.Log("Cmabiando a fight?");
        }
    }
}
