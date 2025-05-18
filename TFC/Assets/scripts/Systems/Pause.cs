using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;  // Asegúrate de que esto esté incluido
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;  // El menú de pausa
    public GameObject firstSelectedButton;  // El primer botón a seleccionar

    private bool isPaused = false;

    void Update()
    {
        // Verifica si se presiona el botón Escape o el botón de pausa del mando
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (isPaused)
            {
                ResumeGame();  // Si el juego está pausado, reanudarlo
            }
            else
            {
                PauseGame();   // Si el juego no está pausado, pausarlo
            }
        }
    }

    // Pausar el juego
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Detiene el tiempo, lo que "pausa" el juego
        pauseMenu.SetActive(true);  // Muestra el menú de pausa (ahora se activa el canvas)
        StartCoroutine(SelectFirstButtonNextFrame());  // Espera un frame para asegurar que el canvas esté activo
    }

    // Reanudar el juego
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Vuelve a la normalidad el tiempo
        pauseMenu.SetActive(false);  // Oculta el menú de pausa (lo desactiva)
        EventSystem.current.SetSelectedGameObject(null);  // Elimina la selección actual del menú
    }

    // Este método asegura que el primer botón se seleccione correctamente
    IEnumerator SelectFirstButtonNextFrame()
    {
        yield return null;  // Esperar un frame para asegurar que el canvas esté activo

        EventSystem.current.SetSelectedGameObject(null);  // Desselecciona cualquier cosa previamente seleccionada
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);  // Selecciona el primer botón

        // Esto puede ayudar a asegurar que el Gamepad puede navegar correctamente
        ExecuteEvents.Execute(firstSelectedButton, new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
    }

    // Método para cambiar a la escena del menú
    public void SalirAlMenu()
    {
        // Asegúrate de que el pauseMenu esté desactivado antes de cargar una nueva escena
        pauseMenu.SetActive(false);

        // Asegura que se complete la transición de la escena antes de realizar cualquier otro cambio
        Debug.Log("Cambiando a la escena: Menu");
        StartCoroutine(ChangeSceneAfterDelay("Menu", 0.5f));  // Retarda la carga para evitar conflictos
    }

    // Método para cambiar de escena con un pequeño retraso
    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);  // Espera un poco antes de cambiar la escena
        LoadingScreenManager.Instance.LoadSceneWithLoading("Menu");
    }
}
