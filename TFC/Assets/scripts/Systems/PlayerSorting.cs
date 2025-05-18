using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int offset = 0; // Por si quieres ajustar manualmente cada tipo de objeto

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // Cuanto más abajo está en pantalla (menor Y), mayor sortingOrder
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100) + offset;
    }
}
