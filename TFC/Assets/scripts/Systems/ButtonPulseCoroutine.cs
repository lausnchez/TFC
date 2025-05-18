using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonPulseOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    private Coroutine pulseCoroutine;
    private bool isPointerOver = false;
    private bool isSelected = false;

    void Start()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    void Update()
    {
        // Inicia o detiene el pulso según estado combinado
        if ((isPointerOver || isSelected) && pulseCoroutine == null)
        {
            pulseCoroutine = StartCoroutine(PulseEffect());
        }
        else if (!isPointerOver && !isSelected && pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
            button.transform.localScale = Vector3.one;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }

    IEnumerator PulseEffect()
    {
        while (true)
        {
            yield return ScaleButton(1.1f, 0.4f);
            yield return ScaleButton(1.0f, 0.4f);
        }
    }

    IEnumerator ScaleButton(float scale, float duration)
    {
        Vector3 startScale = button.transform.localScale;
        Vector3 endScale = new Vector3(scale, scale, 1);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            button.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime; //  Cambio aqui para que el pulso y seleccionable sea tambien en pausa
            yield return null;
        }

        button.transform.localScale = endScale;
    }
}
