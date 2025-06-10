using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    //  panel negro para fade-out
    public CanvasGroup fadePanel;

    // nombre de la siguiente escena
    public string nextSceneName;
    public float fadeDuration = 2f;

    // Start is called before the first frame update
    public void CambiarEscena(string nombreEscena)
    {
        // Carga la escena con el nombre proporcionado
        LoadingScreenManager.Instance.LoadSceneWithLoading("Map");
        Debug.Log($"Cambiando a la escena: {"Map"}");
    }

    // Metodo para salir del juego
    public void SalirDelJuego()
    {
        // Cierra la aplicacion (solo funciona en builds, no en el editor)
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void EmpezarJuego()
    {
        StartCoroutine(FadeOutAndSwitchScene());
    }

    IEnumerator FadeOutAndSwitchScene()
    {

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1f;

        yield return new WaitForSeconds(0.5f); // opcional: pausa antes de cargar

        SceneManager.LoadScene(nextSceneName);
    }
}
