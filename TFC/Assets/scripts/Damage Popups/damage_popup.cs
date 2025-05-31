using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class damage_popup : MonoBehaviour
{
    private TextMeshPro textDamage;
    private float disappearTimer = 0.5f;
    private Color textColor;
    private float moveYSpeed = 1f;
    private float decreaseScale = 0.5f;

    // Crear el PopUp
    public static damage_popup Create(Vector3 position, int dmgAmount)
    {
        Transform dmgPopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        damage_popup damagePopUp = dmgPopupTransform.GetComponent<damage_popup>();
        damagePopUp.Setup(dmgAmount);
        return damagePopUp;
    }

    private void Awake()
    {
        textDamage = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int dmg)
    {
        textDamage.text = dmg.ToString();
        textColor = textDamage.color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed * Time.deltaTime, 0);
        disappearTimer -= Time.deltaTime;

        // Decrease scale effect
        transform.localScale += Vector3.one * decreaseScale * Time.deltaTime;

        // Fade out effect
        if (disappearTimer <= 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textDamage.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject); 
            }
        }
    }
}
