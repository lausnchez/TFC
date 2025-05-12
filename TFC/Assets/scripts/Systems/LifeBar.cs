using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Image FillLifeBar;
    private PlayerStaminaController playerLife;
    private int VidaMaxima;
    // Start is called before the first frame update
    void Start()
    {
        playerLife = GameObject.Find("Player").GetComponent<PlayerStaminaController>();
        VidaMaxima = playerLife.vidaJugador;//con esto cuando compremos mas vida para el jugador no hara falta actualizarla.

    }

    // Update is called once per frame
    void Update()
    {
        FillLifeBar.fillAmount = (float)playerLife.vidaJugador / VidaMaxima;//El Fill amount total es 1 y esto lo que hace es dividir la vida entre ese 1 del inspector de unity
        //lo casteamos a float para que se pueda referenciar bien en la barra de vida ya que es 0 o 1 y si es 0.9 por ejemplo lo trunca a 0

    }
}
