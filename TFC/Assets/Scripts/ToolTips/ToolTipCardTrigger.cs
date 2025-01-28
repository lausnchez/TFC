using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipCardTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    [Multiline()]
    public TMP_Text nombre;
    public TMP_Text descripcion;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(1f, () =>
        {
            ToolTipSystem.Show(descripcion.text, nombre.text);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        ToolTipSystem.Hide();
    }
}
