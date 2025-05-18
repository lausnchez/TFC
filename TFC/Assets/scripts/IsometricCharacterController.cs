using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterController : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 5f; // Velocidad de movimiento
    private Vector2 direccion;
    private Rigidbody2D rb2d;
    //public Animator animator;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");
        //animator.SetFloat("movement", movimientoX);

        // Calcular la dirección del movimiento
        direccion = new Vector2(movimientoX, movimientoY).normalized;

        // Cambiar la escala en el eje X para voltear el sprite cuando va a la izquierda
        if (movimientoX != 0)
        {
            transform.localScale = new Vector3(movimientoX > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        // Mover el personaje en la dirección calculada
        rb2d.MovePosition(rb2d.position + direccion * velocidadMovimiento * Time.fixedDeltaTime);
    }
}
