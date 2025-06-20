using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : MonoBehaviour
{
    [Header("GameObjects")]
    private Enemies.EnemyDataAPI SelectedEnemy;
    public GameObject Player;
    public GameObject turno;
    public TextMeshProUGUI enemyName;
    public EnemyDeathHandler enemyDeathHandler;
    public EnemyLifeBar healthBar;

    public SpriteRenderer spriteRenderer;
    

    private int vidaMaxima;
    private int vida;
    private float enemyPopupOffset = 2.5f;
    private float spriteHeight;
    

    private List<Enemies.EnemyAttack> attack_list = new List<Enemies.EnemyAttack>();    // Daños por turno al jugador
    private List<ActiveEffect> active_enemy_Effects = new List<ActiveEffect>();         // Daños por turno al enemigo

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

    /** 
     * Carga los datos del enemigo desde la API y asigna el sprite correspondiente.
     * @param url URL de la API para cargar los datos del enemigo.
     * @param idEnemy ID del enemigo a cargar.
     */
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

    /*
     * Elige un ataque del enemigo, lo agrega a la lista de ataques y resuelve los daños
     */
    public IEnumerator RealizarAtaques()
    {
        Debug.Log("RealizarAtaques");
        Enemies.EnemyAttack ataque = ElegirAtaque(); // Elige un ataque
        attack_list.Add(ataque); // Añade el ataque a la lista de ataques

        Debug.Log("----- Estado inicial de attack_list -----");
        //foreach (Enemies.EnemyAttack ataquerevisar in attack_list)
        //{
        //    Debug.Log($"Nombre: {ataquerevisar.name}, Valor: {ataquerevisar.value}, Turnos restantes: {ataquerevisar.active_turns}, Turnos iniciales: {ataquerevisar.initial_turns}");
        //}

        StartCoroutine(Attack_Resolve()); // Inicia la resolución de ataques

        Debug.Log("----- Estado final de attack_list -----");
        foreach (Enemies.EnemyAttack ataquerevisar in attack_list)
        {
            Debug.Log($"Nombre: {ataquerevisar.name}, Valor: {ataquerevisar.value}, Turnos restantes: {ataquerevisar.active_turns}, Turnos iniciales: {ataquerevisar.initial_turns}");
        }

        yield return new WaitForSeconds(1f); // Espera 1 segundo entre ataques
    }

    /**
     * Realiza los ataques de la lista de ataques y los retira de la lista en caso de ser necesario
     * Todos los ataques se almacenan aqui, y en caso de ser un 1turn se resuelve inmediatamente y se saca de la lista
     * En caso contrario seguirá ahí hasta que se acaben los turnos de ese ataque
     */
    public IEnumerator Attack_Resolve()
    {
        Debug.Log("AttackResolve");
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
            yield return new WaitForSeconds(1); // Espera 1 segundo entre ataques
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

    /*     
     * Elige un ataque aleatorio del enemigo, e inicializa sus turnos activos.
     * @return Enemies.EnemyAttack: El ataque elegido
     */
    private Enemies.EnemyAttack ElegirAtaque()
    {
        Enemies.EnemyAttack originalAttack = SelectedEnemy.effects[UnityEngine.Random.Range(0, SelectedEnemy.effects.Count)];
        Enemies.EnemyAttack clonedAttack = new Enemies.EnemyAttack()
        {
            id = originalAttack.id,
            name = originalAttack.name,
            description = originalAttack.description,
            initial_turns = originalAttack.initial_turns,
            active_turns = originalAttack.active_turns,
            value = originalAttack.value,
            pivot = originalAttack.pivot
        };
        clonedAttack.initial_turns = clonedAttack.active_turns;
        return clonedAttack;
    }

    /* 
     * Aplica un nuevo efecto activo al enemigo.
     * @param newEffect ActiveEffect: El efecto que se va a aplicar al enemigo.
     */
    public void ApplyNewEffect(ActiveEffect newEffect)
    {
        Debug.Log("ApplyNewEffect");
        active_enemy_Effects.Add(newEffect);
        Debug.Log($"Efecto aplicado: {newEffect.name} con valor {newEffect.value} y {newEffect.turnos_restantes} turnos restantes");
    }

    /* 
     * Aplica los efectos activos al enemigo.
     * Recorre la lista de efectos activos y aplica el daño correspondiente.
     * Si un efecto se agota, se elimina de la lista.
     */
    public void ApplyEffects()
    {
        Debug.Log("ApplyEffects");
        // Se recorre la lista al revés para evitar problemas al eliminar elementos mientras se itera
        for (int i = active_enemy_Effects.Count - 1; i >= 0; i--)
        {
            ActiveEffect effect = active_enemy_Effects[i];
            TakeDamage(effect.value); // Aplica el daño del efecto
            effect.turnos_restantes--; // Reduce los turnos restantes del efecto
            if (effect.turnos_restantes <= 0)
            {
                active_enemy_Effects.RemoveAt(i); // Elimina el efecto si ya no le quedan turnos
                Debug.Log($"Efecto eliminado: {effect.name}");
            }
        }
    }

    /* 
     * Método para que el enemigo haga daño al jugador.
     * @param damage int: La cantidad de daño que recibe el enemigo.
     */
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

    /*
     * Método que maneja la muerte del enemigo.
     * Destruye el objeto del enemigo y notifica al EnemyDeathHandler.
     */
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