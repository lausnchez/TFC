using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : MonoBehaviour
{
    private Enemies.EnemyDataAPI SelectedEnemy;
    public GameObject Player;
    public GameObject turno;

    public SpriteRenderer spriteRenderer;
    public EnemyDeathHandler enemyDeathHandler;
    public EnemyLifeBar healthBar;
    private int vidaMaxima;
    private int vida;
    private float enemyPopupOffset = 2.5f;
    private float spriteHeight;
    public TextMeshProUGUI enemyName;

    // Lista de daños por turno
    private List<Enemies.EnemyAttack> attack_list = new List<Enemies.EnemyAttack>();
    public StatusEffectVisualizer statusEffectVisualizer;

    void Start()
    {
        // Busca el GameObject que tiene el script EnemyDeathHandler
        enemyDeathHandler = FindObjectOfType<EnemyDeathHandler>();  
        if (enemyDeathHandler == null)
        {
            Debug.LogError("No se encontró un EnemyDeathHandler en la escena.");
        }

        // Elegir enemigo aleatorio
        System.Random random = new System.Random();
        int enemyNumber = random.Next(1, 4); // Genera un número aleatorio entre 1 y 3 (inclusive)
        StartCoroutine(loadEnemyData("https://tfgvideojuego.lausnchez.es/api/enemy", enemyNumber));

        // Inicializar el status effect visualizer
        if (statusEffectVisualizer == null)
        {
            GameObject effectCanvas = GameObject.Find("EffectIconCanvas");
            statusEffectVisualizer = effectCanvas.GetComponentInChildren<StatusEffectVisualizer>();
        }
    }

    private IEnumerator loadEnemyData(string url, int idEnemy)
    {
        string urlWithID = url + "/" + idEnemy;
        using (UnityWebRequest www = UnityWebRequest.Get(urlWithID))//creamos una peticion a la api
        {
            yield return www.SendWebRequest();//esperamos que termine la peticion

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error cargando API: " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                SelectedEnemy = JsonUtility.FromJson<Enemies.EnemyDataAPI>(json);
                   
                Sprite spriteEnemigo = Resources.Load<Sprite>("Enemies/" + SelectedEnemy.sprite);
                if (spriteEnemigo != null)
                {
                    try
                    {
                        spriteRenderer.sprite = spriteEnemigo; // Asignar el sprite al SpriteRenderer
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error al asignar el sprite: " + e.Message);
                    }
                }
                else Debug.LogError("No se pudo cargar el sprite del enemigo: " + SelectedEnemy.sprite);

                // Cambiar parámetros del enemigo
                vidaMaxima = SelectedEnemy.health;
                vida = vidaMaxima;
                enemyName.text = SelectedEnemy.name;
                Debug.Log("Num de ataques:" + SelectedEnemy.effects.Count);

                // Setear la barra de vida
                spriteHeight = spriteRenderer.bounds.size.y; // Obtener la altura del sprite
                healthBar.SetHealth(vida, vidaMaxima,this.transform, spriteHeight);
            }
        }
    }
    // Metodo para que el enemigo realice 3 ataques
    public IEnumerator RealizarAtaques()
    {
        Enemies.EnemyAttack ataque = ElegirAtaque(); // Elige un ataque
        attack_list.Add(ataque); // Añade el ataque a la lista de ataques

        Debug.Log("----- Estado inicial de attack_list -----");
        foreach (Enemies.EnemyAttack ataquerevisar in attack_list)
        {
            Debug.Log($"Nombre: {ataquerevisar.name}, Valor: {ataquerevisar.value}, Turnos restantes: {ataquerevisar.active_turns}, Turnos iniciales: {ataquerevisar.initial_turns}");
        }


        StartCoroutine(Attack_Resolve()); // Inicia la resolución de ataques

        Debug.Log("----- Estado final de attack_list -----");
        foreach (Enemies.EnemyAttack ataquerevisar in attack_list)
        {
            Debug.Log($"Nombre: {ataquerevisar.name}, Valor: {ataquerevisar.value}, Turnos restantes: {ataquerevisar.active_turns}, Turnos iniciales: {ataquerevisar.initial_turns}");
        }

        yield return new WaitForSeconds(1f); // Espera 1 segundo entre ataques
    }

    // Realiza los ataques y los retira de la lista de ataques en caso de ser necesario
    // Todos los ataques se almacenan aqui, y en caso de ser un 1turn se resuelve inmediatamente y se saca de la lista
    // En caso contrario seguirá ahí hasta que se acaben los turnos de ese ataque
    public IEnumerator Attack_Resolve()
    {
        // Realizar los daños
        List<Enemies.EnemyAttack> ataques_a_eliminar = new List<Enemies.EnemyAttack>();
        foreach (Enemies.EnemyAttack ataque_realizado in attack_list)
        {
            // Daño al player
            PlayerStaminaController.Instance.TakeDamage(ataque_realizado.value);

            // Crear popup de daño
            Vector3 playerDamage_Offset = new Vector3(0.5f, 2.2f, 0); // Offset para el popup de daño del jugador
            damage_popup.Create(GameAssets.i.Player.position + playerDamage_Offset, ataque_realizado.value.ToString(), "player", ataque_realizado.name);

            // Comprobar si hay algún ataque al que no le quedan turnos restantes
            ataque_realizado.active_turns--;
            if (ataque_realizado.active_turns <= 0)
            {
                ataques_a_eliminar.Add(ataque_realizado); // Añade el ataque a la lista de ataques a eliminar si ya no tiene turnos restantes
                Debug.Log($"Ataque a eliminar: {ataque_realizado.name}");
            }
            yield return new WaitForSeconds(0.5f); // Espera 1 segundo entre ataques
        }
        // Quitar los ataques que ya no tienen turnos restantes
        foreach (Enemies.EnemyAttack ataque_eliminado in ataques_a_eliminar)
        {
            Vector3 playerDamage_Offset = new Vector3(0.5f, 2.2f, 0); // Offset para el popup de daño del jugador
            if (ataque_eliminado.initial_turns != 1)
            {
                damage_popup.Create(GameAssets.i.Player.position + playerDamage_Offset, "<size=1>" + ataque_eliminado.name + " terminado</size>", "player");
            }
            attack_list.Remove(ataque_eliminado);
            Debug.Log($"Ataque eliminado: {ataque_eliminado.name} / {ataque_eliminado.active_turns} turnos");
        }

        // Mostrar los iconos de status
        statusEffectVisualizer.UpdateIcons(attack_list); // Actualiza los iconos de efectos en el visualizador
        //GameAssets.i.Player.GetComponent<StatusEffectVisualizer>().UpdateIcons(attack_list);
    }

    // Metodo para elegir un ataque basado en las probabilidades
    private Enemies.EnemyAttack ElegirAtaque()
    {
        Enemies.EnemyAttack attack = SelectedEnemy.effects[UnityEngine.Random.Range(0, SelectedEnemy.effects.Count)];
        attack.initial_turns = attack.active_turns;
        Debug.Log($"Ataque elegido: {attack.name} (ID: {attack.id}) con valor {attack.value} y {attack.initial_turns} turnos restantes");
        return attack;
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
        healthBar.SetHealth(vida, vidaMaxima, this.transform, spriteHeight); // Actualiza la barra de vida
        Debug.Log("Enemigo recibio daño. Vida restante: " + vida);

        Vector3 offset = new Vector3(0, enemyPopupOffset, 0); // Offset para el popup de daño
        damage_popup.Create(transform.position + offset, damage.ToString(),"enemy");

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