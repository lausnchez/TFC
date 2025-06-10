using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    [SerializeField] private GameObject loadingScreenPrefab;

    private GameObject loadingScreenInstance;
    private RawImage displayImage;
    private Texture[] images;
    private AudioSource audioSource;

    private RectTransform spinnerTransform;
    private TextMeshProUGUI loadingText;

    public float imageDuration = 2f;
    public float fadeDuration = 2f;

    private float loadProgress = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(PrepareAndLoad(sceneName));
    }

    IEnumerator PrepareAndLoad(string sceneName)
    {
        // Mostrar pantalla de carga inmediatamente
        if (loadingScreenInstance == null)
        {
            loadingScreenInstance = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreenInstance);

            displayImage = loadingScreenInstance.GetComponentInChildren<RawImage>();
            spinnerTransform = loadingScreenInstance.transform.Find("LoadingSpinner").GetComponent<RectTransform>();
            loadingText = loadingScreenInstance.transform.Find("LoadingText").GetComponent<TextMeshProUGUI>();
            loadingText.text = "Cargando";

            SetupAudio();
            StartCoroutine(AnimateLoadingText());
        }

        yield return null; // Esperar un frame para que se muestre la pantalla

        // Detener música del menú principal si existe
        GameObject Menu = GameObject.Find("Menu");
        if (Menu != null)
        {
            AudioSource menuAudio = Menu.GetComponent<AudioSource>();
            if (menuAudio != null && menuAudio.isPlaying)
            {
                menuAudio.Stop();
            }
        }

        // Cargar imágenes y empezar carga de escena
        images = Resources.LoadAll<Texture>("LoadingImages");
        ShuffleArray(images);

        yield return StartCoroutine(LoadSceneAsyncWithImages(sceneName));
    }

    void SetupAudio()
    {
        if (audioSource == null)
        {
            audioSource = loadingScreenInstance.GetComponent<AudioSource>();
            AudioClip clip = Resources.Load<AudioClip>("Audio/loading_theme");

            if (audioSource != null && clip != null)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                StartCoroutine(FadeInAudio(fadeDuration));
            }
            else
            {
                Debug.LogWarning("AudioSource o AudioClip no encontrado.");
            }
        }
    }

    IEnumerator LoadSceneAsyncWithImages(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        int index = 0;
        while (!asyncLoad.isDone)
        {
            if (index < images.Length)
            {
                displayImage.texture = images[index];
                index++;
            }

            loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                loadProgress = 1f;
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(imageDuration);
        }

        yield return StartCoroutine(FadeOutAudio(fadeDuration));

        Destroy(loadingScreenInstance);
        loadingScreenInstance = null;
        audioSource = null;
    }

    void Update()
    {
        if (spinnerTransform != null)
        {
            float rotationSpeed = Mathf.Lerp(150f, 600f, loadProgress);
            spinnerTransform.Rotate(Vector3.forward, -rotationSpeed * Time.unscaledDeltaTime);
        }
    }

    void ShuffleArray(Texture[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Texture temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        Debug.Log("Imágenes mezcladas aleatoriamente:");
        foreach (var tex in array)
        {
            Debug.Log(tex.name);
        }
    }

    IEnumerator FadeInAudio(float duration)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    IEnumerator AnimateLoadingText()
    {
        string baseText = "Cargando";
        int dotCount = 0;

        while (loadingScreenInstance != null)
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
