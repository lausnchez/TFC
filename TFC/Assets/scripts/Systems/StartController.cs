using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    // Start is called before the first frame update
    public void CambiarEscena(string nombreEscena)
    {
        // Carga la escena con el nombre proporcionado
        SceneManager.LoadScene("FIGHT");
        Debug.Log($"Cambiando a la escena: {"FIGHT"}");
    }

    // Metodo para salir del juego
    public void SalirDelJuego()
    {
        // Cierra la aplicacion (solo funciona en builds, no en el editor)
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
