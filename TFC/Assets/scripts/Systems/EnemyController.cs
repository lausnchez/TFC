using System.Collections;
using System.Data;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public GameObject turno;

    public EnemyDeathHandler enemyDeathHandler;
    public EnemyLifeBar healthBar;
    public int vidaMaxima = 33; // Vida máxima del enemigo
    public int vida = 33; // Vida del enemigo
    private float enemyPopupOffset = 2.5f;
    public TextMeshProUGUI enemyName;

    // Probabilidades de cada ataque (en porcentajes)
    private int probabilidadAtaque2 = 50; // 50% de probabilidad
    private int probabilidadAtaque5 = 30; // 30% de probabilidad
    private int probabilidadAtaque7 = 20; // 20% de probabilidad

    // Daños de cada ataque
    private int danoAtaque2 = 2;
    private int danoAtaque5 = 5;
    private int danoAtaque7 = 7;

    void Start()
    {
        // Busca el GameObject que tiene el script EnemyDeathHandler
        enemyDeathHandler = FindObjectOfType<EnemyDeathHandler>();
        healthBar.SetHealth(vida, vidaMaxima);
        enemyName.text = "Esqueleto";

        if (enemyDeathHandler == null)
        {
            Debug.LogError("No se encontró un EnemyDeathHandler en la escena.");
        }
    }
    // Metodo para que el enemigo realice 3 ataques
    public IEnumerator RealizarAtaques()
    {
        for (int i = 0; i < 3; i++)
        {
            int ataque = ElegirAtaque(); // Elige un ataque basado en las probabilidades
            int dano = ObtenerDano(ataque); // Obtiene el daño del ataque elegido

            Debug.Log($"Enemigo ataca con el ataque {ataque}. Daño infligido: {dano}");
            PlayerStaminaController.Instance.TakeDamage(dano); // Inflige daño al jugador
            yield return new WaitForSeconds(1f); // Espera 1 segundo entre ataques
        }

        Debug.Log("El enemigo ha terminado sus ataques.");
    }

    // Metodo para elegir un ataque basado en las probabilidades
    private int ElegirAtaque()
    {
        int random = Random.Range(0, 100); // Genera un numero aleatorio entre 0 y 99

        if (random < probabilidadAtaque2)
        {
            return 2; // Ataque de 2 de daño
        }
        else if (random < probabilidadAtaque2 + probabilidadAtaque5)
        {
            return 5; // Ataque de 5 de daño
        }
        else
        {
            return 7; // Ataque de 7 de daño
        }
    }

    // Metodo para obtener el daño del ataque elegido
    private int ObtenerDano(int ataque)
    {
        switch (ataque)
        {
            case 2:
                return danoAtaque2;
            case 5:
                return danoAtaque5;
            case 7:
                return danoAtaque7;
            default:
                return 0; // Si no se elige un ataque valido, no hace daño
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            Debug.Log("¡La carta golpeó al enemigo!");
            // Lógica para recibir daño o efectos de la carta
            TakeDamage(10);
        }
    }

    // Metodo para recibir daño
    public void TakeDamage(int damage)
    {
        vida -= damage; // Reduce la vida
        healthBar.SetHealth(vida, vidaMaxima); // Actualiza la barra de vida
        Debug.Log("Enemigo recibio daño. Vida restante: " + vida);

        Vector3 offset = new Vector3(0, enemyPopupOffset, 0); // Offset para el popup de daño
        damage_popup.Create(transform.position + offset, damage);

        if (vida <= 0)
        {
            Die(); // Llama al método para manejar la muerte del enemigo
        }
    }

    // Metodo para manejar la muerte del enemigo
    public void Die()
    {
        if (vida <= 0)
        {
            Debug.Log("Enemigo derrotado.");
            enemyDeathHandler.OnEnemyDefeated();
            Destroy(gameObject);
            Destroy(Player);
            Destroy(turno);
            Destroy(healthBar);
            //Destroy(Map.Enemigo1);Esto no funciona bien ya que necesito crear una clase Map y asignar en el start de la escena map
            //la referencia al enemigo1 que es en este caso el que quiero destruir ya que necesitamos destruirlo para poder avanzar al siguiente lvl

        }
    }
}