using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class damage_popup : MonoBehaviour
{
    private TextMeshPro textDamage;
    private float disappearTimer = 0.5f;
    private Color textColor;
    private float moveYSpeed = 1f;
    private float decreaseScale = 0.5f;
    private string popup_target; // "player" or "enemy"

    // Crear el PopUp
    public static damage_popup Create(Vector3 position, string dmgAmount, string target, string attackName = "")
    {
        Transform dmgPopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        damage_popup damagePopUp = dmgPopupTransform.GetComponent<damage_popup>();
        damagePopUp.Setup(dmgAmount, attackName, target);
        return damagePopUp;
    }

    public static damage_popup CreateText(Vector3 position, string dmgAmount = "")
    {
        Transform dmgPopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        damage_popup damagePopUp = dmgPopupTransform.GetComponent<damage_popup>();
        damagePopUp.SetupOnlyText(dmgAmount);
        return damagePopUp;
    }

    public void SetupOnlyText(string text)
    {
        string popup_text = $"<size=1.2>{text}</size>";
        disappearTimer = 0.75f;
        moveYSpeed = 0f;
        textDamage.text = popup_text;
        textColor = textDamage.color;
    }

    private void Awake()
    {
        textDamage = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string dmg, string attackName, string target)
    {
        if (target == "player")
        {
            textDamage.color = Color.red;
            string dmg_string = $"<size=1.4>{attackName}</size>\n{dmg}";
            textDamage.text = dmg_string;
        }
        else
        {
            textDamage.text = dmg;
        }
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
