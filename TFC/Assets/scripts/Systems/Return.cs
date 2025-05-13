using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    // Start is called before the first frame update
    public void Volver()
    {
        LoadingScreenManager.Instance.LoadSceneWithLoading("Map");
        Debug.Log("Cmabiando a Map?");

    }
}
