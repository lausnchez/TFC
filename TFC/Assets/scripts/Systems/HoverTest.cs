using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HoverTest : MonoBehaviour
{
    public GameObject borderObject;
    public GameObject Knife;

    [HideInInspector] public Vector3 originalScale = Vector3.one;
    private float originalZ;

    public float hoverScale = 0.75f;
    public float scaleSpeed = 5f;
    public float hoverZOffset = -0.2f;

    private Tween wiggleTween;

    void Start()
    {
        originalScale = transform.localScale;
        originalZ = transform.position.z;

        if (borderObject != null)
            borderObject.SetActive(false);
        if (Knife != null)
            Knife.SetActive(false);
    }

    void OnMouseEnter()
    {
        ActivateHover();
    }

    void OnMouseExit()
    {
        DeactivateHover();
    }

    public void ActivateHover()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(hoverScale));

        // Activar borde (glow)
        if (borderObject != null)
            borderObject.SetActive(true);
        Knife.SetActive(true);

        // Subir en eje Z para que esté delante
        Vector3 pos = transform.position;
        pos.z = originalZ + hoverZOffset;
        transform.position = pos;

        // Aplicar efecto de wiggle (vibración)
        if (wiggleTween != null && wiggleTween.IsActive()) wiggleTween.Kill();
        wiggleTween = transform.DOShakeRotation(
            duration: 0.5f,
            strength: new Vector3(0, 0, 5), // solo rotación en Z
            vibrato: 7,
            randomness: 90,
            fadeOut: false,
            randomnessMode: ShakeRandomnessMode.Full
        );
    }

    public void DeactivateHover()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(1f));

        // Desactivar glow
        if (borderObject != null)
            borderObject.SetActive(false);
        Knife.SetActive(false);

        // Volver al z original
        Vector3 pos = transform.position;
        pos.z = originalZ;
        transform.position = pos;

        // Detener vibración
        if (wiggleTween != null && wiggleTween.IsActive()) wiggleTween.Kill();
        transform.rotation = Quaternion.identity; // reset rotacion
    }

    private IEnumerator ScaleTo(float targetScale)
    {
        float timeElapsed = 0f;
        Vector3 initialScale = transform.localScale;

        while (timeElapsed < 1f)
        {
            transform.localScale = Vector3.Lerp(initialScale, originalScale * targetScale, timeElapsed);
            timeElapsed += Time.deltaTime * scaleSpeed;
            yield return null;
        }

        transform.localScale = originalScale * targetScale;
    }
}
