using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Para Image

public class RestrictedZoneMessage : MonoBehaviour
{
    public GameObject canvas;           // Canvas completo
    public Image panelImage;            // Imagen del panel a desvanecer
    public float fadeDuration = 1f;     // Duración del desvanecido

    public TextPulse textPulse;         // Referencia al script de pulso en el texto

    private bool isMessageVisible = false;

    private void Start()
    {
        // Asegurarse de que el canvas esté desactivado al inicio
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detener el tiempo del juego y activar el canvas
            Time.timeScale = 0f;
            canvas.SetActive(true);

            // Asegurar que el panel sea completamente opaco
            Color color = panelImage.color;
            color.a = 1f;
            panelImage.color = color;

            // Iniciar el pulso del texto
            textPulse?.StartPulse();

            isMessageVisible = true;
        }
    }

    private void Update()
    {
        // Comprobar si el jugador ha hecho clic para cerrar el mensaje
        if (isMessageVisible && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            StartCoroutine(FadeOutAndResume());
        }
    }

    private IEnumerator FadeOutAndResume()
    {
        isMessageVisible = false;

        float elapsed = 0f;
        Color startColor = panelImage.color;

        // Desvanecer el panel
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;  // Usar el tiempo no escalado debido a Time.timeScale = 0
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            panelImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // Asegurar que el panel esté completamente transparente
        panelImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Detener el pulso del texto
        textPulse?.StopPulse();

        // Desactivar el canvas y restaurar el tiempo del juego
        canvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
