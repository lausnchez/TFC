using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject victoryBanner;
    public RectTransform victoryRect;
    public Image blackFade;
    public float fadeDuration = 1.5f;
    public float timeBeforeSceneChange = 2.5f;
    public string nextSceneName;

    private bool isDefeated = false;
    private float fadeTimer = 0f;
    public GameObject Player;
    public GameObject Audio;

    void Start()
    {
        victoryBanner.SetActive(false);
        Color c = blackFade.color;
        c.a = 0f;
        blackFade.color = c;
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Destroy");
        }
    }

    public void OnEnemyDefeated()
    {
        isDefeated = true;
        Destroy(Player);
        StartCoroutine(AnimateVictoryBanner());
        StartCoroutine(ChangeSceneAfterDelay(nextSceneName, fadeDuration + timeBeforeSceneChange));
    }

    void Update()
    {
        if (isDefeated && fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            Color c = blackFade.color;
            c.a = alpha;
            blackFade.color = c;
        }
    }

    private IEnumerator AnimateVictoryBanner()
    {
        victoryBanner.SetActive(true);
        victoryRect.localScale = Vector3.zero;
        victoryRect.anchoredPosition = new Vector2(0, -Screen.height / 2f); // Punto de fuga desde abajo, se podria cambiar al medio

        float duration = 1f;
        float timer = 0f;

        Vector3 targetScale = Vector3.one;
        Vector2 targetPosition = Vector2.zero;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            t = Mathf.SmoothStep(0, 1, t); // Hace la animación mas fluida

            victoryRect.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            victoryRect.anchoredPosition = Vector2.Lerp(new Vector2(0, -Screen.height / 2f), targetPosition, t);

            yield return null;
        }

        victoryRect.localScale = targetScale;
        victoryRect.anchoredPosition = targetPosition;
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(victoryBanner);//destruye el banner para que no siga apareciendo en otras escenas o en la pantalla de carga
        Destroy(blackFade);//destruye el fundido a negro para lo mismo que el anterior
                           // Destroy(Player);
        Destroy(Audio);
        LoadingScreenManager.Instance.LoadSceneWithLoading(sceneName);
        //Destroy(Player);
    }
}
