using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float x = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float y = PlayerPrefs.GetFloat("PlayerPosY", 0f);

        transform.position = new Vector3(x, y, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
