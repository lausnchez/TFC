using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int vida = 12; // Vida del enemigo

    // Probabilidades de cada ataque (en porcentajes)
    private int probabilidadAtaque2 = 50; // 50% de probabilidad
    private int probabilidadAtaque5 = 30; // 30% de probabilidad
    private int probabilidadAtaque7 = 20; // 20% de probabilidad

    // Daños de cada ataque
    private int danoAtaque2 = 2;
    private int danoAtaque5 = 5;
    private int danoAtaque7 = 7;

    // Método para que el enemigo realice 3 ataques
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

    // Método para elegir un ataque basado en las probabilidades
    private int ElegirAtaque()
    {
        int random = Random.Range(0, 100); // Genera un número aleatorio entre 0 y 99

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

    // Método para obtener el daño del ataque elegido
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
                return 0; // Si no se elige un ataque válido, no hace daño
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        vida -= damage; // Reduce la vida
        Debug.Log("Enemigo recibió daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Die(); // Llama al método para manejar la muerte del enemigo
        }
    }

    // Método para manejar la muerte del enemigo
    public void Die()
    {
        if (vida <= 0)
        {
            Debug.Log("Enemigo derrotado.");
            Destroy(gameObject);
        }
    }
}