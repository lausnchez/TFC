using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStaminaController : MonoBehaviour
{
    public static PlayerStaminaController Instance { get; private set; } // Singleton

    [Header("Stamina Settings")]
    public int maxStamina;
    public int currentStamina;
    public int staminaCostPerCard = 20;

    [Header("Vida del Jugador")]
    public int vidaJugador;
    private int maxVidaJugador;
    public TextMeshProUGUI playerLifeCounter;

    [Header("Regeneracion de Stamina")]
    //public ManaSystem manaSystem;

    public float staminaRegenRate = 5f;
    private float regenCooldown = 2f;
    private float lastStaminaUseTime;

    private bool isPlayerTurn = true; // Controla de quien es el turno

    public EnemyController enemyController;

    private void Awake()
    {
        // Configura el Singleton
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Opcional: Para mantener el objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Recoger datos del character
        //manaSystem = GameAssets.i.ManaDisplay.GetComponent<ManaSystem>();
        InitializePlayer initPlayer = GetComponent<InitializePlayer>();

        if (initPlayer != null)
        {
            initPlayer.StartCreateCharacter("https://tfgvideojuego.lausnchez.es/api/character", 1);    
        }
        else
        {
            Debug.LogWarning("No se encontró InitializePlayer en este GameObject.");
        }    
    }

    private void Start()
    {
        //manaSystem.InicializarMana(7);
    }

    /**
     * 
     */
    public void InicializarJugador(int vida, int stamina)
    {
        vidaJugador = vida;
        maxStamina = stamina;
        currentStamina = maxStamina;
        Debug.Log($"Player: {vidaJugador}(vida), {currentStamina}/{maxStamina}(mana)");
    }

    public bool CanUseCard(int mana)
    {
        return true;
        //return manaSystem.canUseMana(mana);
    }

    public void UseCard(int mana)
    {
        if (CanUseCard(mana))
        {
            currentStamina -= staminaCostPerCard;
            lastStaminaUseTime = Time.time;
            Debug.Log($"Carta usada. Stamina restante: {currentStamina}");
        }
        else
        {
            Debug.Log("No puedes usar cartas en este momento o no tienes suficiente stamina.");
        }
    }

    private void RegenerateStamina()
    {
        currentStamina = Mathf.Min(currentStamina + (int)(staminaRegenRate * Time.deltaTime), maxStamina);
        Debug.Log($"Regenerando stamina. Stamina actual: {currentStamina}");
    }

    // Metodo para terminar el turno del jugador
    public void EndTurn()
    {
        if (isPlayerTurn)
        {
            Debug.Log("Terminando turno del jugador...");
            isPlayerTurn = false; // Cambia al turno del enemigo
            StartCoroutine(EnemyTurn()); // Inicia la simulacion del turno del enemigo
            //GameAssets.i.ManaDisplay.GetComponent<ManaSystem>().ResetMana(); // Resetea el mana al final del turno del jugador
        }
    }

    // Corrutina para simular el turno del enemigo
    private IEnumerator EnemyTurn()
    {
        Debug.Log("Es el turno del enemigo. Reseteando mana. Realizando ataques...");
        ManaSystem.Instance.ResetMana(); // Resetea el mana al inicio del turno del enemigo
        yield return enemyController.StartCoroutine(enemyController.RealizarAtaques()); // Realiza los ataques del enemigo

        Debug.Log("Turno del enemigo terminado. Reiniciando estamina y comenzando tu turno.");
        isPlayerTurn = true; // Vuelve al turno del jugador

        // Mostrar la vida restante del jugador
        
        Debug.Log($"Vida restante del jugador: {vidaJugador}");

        // Verificar si el jugador ha muerto
        if (vidaJugador <= 0)
        {
            Die(); // Llama al metodo Die si la vida del jugador es 0 o menos
        }
    }

    /**
     * Hacer cierta cantidad daño al player
     */
    public void TakeDamage(int damage)
    {
        vidaJugador -= damage; // Reduce la vida del jugador
        Debug.Log($"Jugador recibio {damage} de daño. Vida restante: {vidaJugador}");
        actualizarLifeCounter();
        if (vidaJugador <= 0)
        {
            Die(); // Llama al metodo Die si la vida del jugador es 0 o menos
        }
    }

    // Metodo para manejar la muerte del jugador
    public void Die()
    {
        Debug.Log("Has muerto!");
        Destroy(gameObject);
        // Aqui puedes añadir logica adicional, como reiniciar el nivel o mostrar una pantalla de Game Over.
    }

    public void actualizarLifeCounter()
    {
        playerLifeCounter.text = vidaJugador.ToString(); // Actualiza el contador de vida del jugador
    }
}