using System.Collections;
using UnityEngine;

public class PlayerStaminaController : MonoBehaviour
{
    public static PlayerStaminaController Instance { get; private set; } // Singleton

    [Header("Stamina Settings")]
    public int maxStamina = 100;
    public int currentStamina;
    public int staminaCostPerCard = 20;

    [Header("Vida del Jugador")]
    public int vidaJugador = 20; // Vida del jugador

    [Header("Regeneración de Stamina")]
    public float staminaRegenRate = 5f;
    private float regenCooldown = 2f;
    private float lastStaminaUseTime;

    private bool isPlayerTurn = true; // Controla de quién es el turno

    public EnemyController enemyController; // Referencia al EnemyController

    private void Awake()
    {
        // Configura el Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: Para mantener el objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentStamina = maxStamina; // Inicia con la estamina llena
    }

    private void Update()
    {
        // Regeneración de stamina durante el turno del jugador
        if (isPlayerTurn && Time.time - lastStaminaUseTime >= regenCooldown && currentStamina < maxStamina)
        {
            RegenerateStamina();
        }
    }

    public bool CanUseCard()
    {
        return isPlayerTurn && currentStamina >= staminaCostPerCard;
    }

    public void UseCard()
    {
        if (CanUseCard())
        {
            currentStamina -= staminaCostPerCard;
            lastStaminaUseTime = Time.time;
            Debug.Log($"Carta usada. Stamina restante: {currentStamina}");
        }
        else
        {
            Debug.Log("No puedes usar cartas en este momento o no tienes suficiente estamina.");
        }
    }

    private void RegenerateStamina()
    {
        currentStamina = Mathf.Min(currentStamina + (int)(staminaRegenRate * Time.deltaTime), maxStamina);
        Debug.Log($"Regenerando stamina. Stamina actual: {currentStamina}");
    }

    // Método para terminar el turno del jugador
    public void EndTurn()
    {
        if (isPlayerTurn)
        {
            Debug.Log("Terminando turno del jugador...");
            isPlayerTurn = false; // Cambia al turno del enemigo
            StartCoroutine(EnemyTurn()); // Inicia la simulación del turno del enemigo
        }
    }

    // Corrutina para simular el turno del enemigo
    private IEnumerator EnemyTurn()
    {
        Debug.Log("Es el turno del enemigo. Realizando ataques...");
        yield return enemyController.StartCoroutine(enemyController.RealizarAtaques()); // Realiza los ataques del enemigo

        Debug.Log("Turno del enemigo terminado. Reiniciando estamina y comenzando tu turno.");
        currentStamina = maxStamina; // Reinicia la estamina al máximo
        isPlayerTurn = true; // Vuelve al turno del jugador

        // Mostrar la vida restante del jugador
        Debug.Log($"Vida restante del jugador: {vidaJugador}");

        // Verificar si el jugador ha muerto
        if (vidaJugador <= 0)
        {
            Die(); // Llama al método Die si la vida del jugador es 0 o menos
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        vidaJugador -= damage; // Reduce la vida del jugador
        Debug.Log($"Jugador recibió {damage} de daño. Vida restante: {vidaJugador}");

        if (vidaJugador <= 0)
        {
            Die(); // Llama al método Die si la vida del jugador es 0 o menos
        }
    }

    // Método para manejar la muerte del jugador
    public void Die()
    {
        Debug.Log("¡Has muerto!");
        Destroy(gameObject);
        // Aquí puedes añadir lógica adicional, como reiniciar el nivel o mostrar una pantalla de Game Over.
    }
}