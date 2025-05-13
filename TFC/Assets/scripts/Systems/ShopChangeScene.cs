using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChangeScene : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadingScreenManager.Instance.LoadSceneWithLoading("Shop");
            Debug.Log("Cmabiando a Shopp?");
        }
    }
}
