using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonPulseCoroutine : MonoBehaviour
{
    public Button button;
    private bool isPulsing = true;

    void Start()
    {
        StartCoroutine(PulseEffect());
    }

    IEnumerator PulseEffect()
    {
        while (isPulsing)
        {
            yield return ScaleButton(1.1f, 0.5f);
            yield return ScaleButton(1.0f, 0.5f);
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.transform.localScale = endScale;
    }
}