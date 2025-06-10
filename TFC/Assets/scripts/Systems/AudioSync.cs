using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // ← Para cargar la siguiente escena

public class AudioSync : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshProUGUI dialogueText;

    //  panel negro para fade-out
    public CanvasGroup fadePanel;

    // nombre de la siguiente escena
    public string nextSceneName;

    private string[] lines = new string[]
    {
        "Desde que el gobierno me arrancó el vino de las manos, empecé a ver cosas, capisce? No es que me volví loco... es que ahora veo la ciudad como es de verdad.",
        "Anoche, caminaba por Mulberry Street... las farolas parpadeaban como si no quisieran mostrarme algo. Y lo vi. Un tipo, alto como un poste, con ojos de carbón fundido y una piel... como cuero mojado. No era humano. No hablaba. Me miró... como si supiera mi nombre desde antes que mi madre me lo diera.",
        "Pensé que eran visiones, que era el insomnio... pero el ruido de sus pasos no lo soñé. Ni el fuego azul que dejó en el suelo. La ciudad ya no pertenece a hombres como yo... está cambiando, tomando otra forma.",
        "Los chicos del barrio dicen que ando paranoico desde que no puedo tomar ni una maldita grappa. Pero yo sé lo que vi. Las alcantarillas respiran. Los taxis no tienen conductores. Y en el cielo... hay luces que no son de este mundo.",
        "No sé si es castigo o revelación. Pero esta ciudad... esta Nueva York... ya no es nuestra. Y si estos monstruos quieren guerra, que vengan. Porque aunque me falte el vino, aún tengo la pólvora."
    };

    private float[] showTimes = new float[]
    {
        0f,
        12.5f,
        42f,
        60f,
        79f
    };

    private int currentLineIndex = 0;
    public float fadeDuration = 2f;

    void Start()
    {
        dialogueText.color = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);
        if (fadePanel != null)
            fadePanel.alpha = 0f; // Inicialmente invisible
    }

    void Update()
    {
        if (currentLineIndex < showTimes.Length && audioSource.time >= showTimes[currentLineIndex])
        {
            StopAllCoroutines();
            dialogueText.text = lines[currentLineIndex];
            SetAlpha(1f);
            currentLineIndex++;

            if (currentLineIndex < showTimes.Length)
            {
                float timeToNextLine = showTimes[currentLineIndex] - audioSource.time;
                StartCoroutine(FadeOutRoutine(Mathf.Max(0, timeToNextLine - fadeDuration)));
            }
            else
            {
                // Fade del texto, luego fade to black y cambio de escena
                StartCoroutine(FadeOutAndSwitchScene());
            }
        }
    }

    IEnumerator FadeOutRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        Color c = dialogueText.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0f);
    }

    IEnumerator FadeOutAndSwitchScene()
    {
        // Fade del texto
        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(FadeOutRoutine(0f));

        // Espera un poco y empieza el fade a negro
        yield return new WaitForSeconds(1f);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1f;

        // Espera y carga escena
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }

    void SetAlpha(float alpha)
    {
        Color c = dialogueText.color;
        dialogueText.color = new Color(c.r, c.g, c.b, alpha);
    }
}
