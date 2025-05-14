using UnityEngine;
using System.Collections;

public class TextPulse : MonoBehaviour
{
    public float pulseSpeed = 2f;      // Velocidad del pulso
    public float pulseAmount = 0.1f;   // Intensidad del pulso

    private Vector3 originalScale;     // Escala original del texto
    private Coroutine pulseRoutine;    // Corutina del pulso

    private void Awake()
    {
        originalScale = transform.localScale;  // Guardar escala original
    }

    // Iniciar el pulso
    public void StartPulse()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);  // Detener cualquier corutina anterior

        pulseRoutine = StartCoroutine(Pulse());
    }

    // Detener el pulso
    public void StopPulse()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            transform.localScale = originalScale;  // Restablecer la escala original
        }
    }

    // Corutina del pulso
    private IEnumerator Pulse()
    {
        float timer = 0f;

        // Pulsar indefinidamente
        while (true)
        {
            float scale = 1f + Mathf.Sin(timer) * pulseAmount;
            transform.localScale = originalScale * scale;

            timer += Time.unscaledDeltaTime * pulseSpeed; // Usar Time.unscaledDeltaTime para funcionar con Time.timeScale = 0
            yield return null;
        }
    }
}
