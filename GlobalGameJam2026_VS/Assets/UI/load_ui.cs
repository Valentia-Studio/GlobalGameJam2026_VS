using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_ui : MonoBehaviour
{
    [Tooltip("Nombre de la escena a cargar (añadir a Build Settings)")]
    [SerializeField] private string sceneToLoad = "UIScene";

    [Tooltip("Segundos de espera antes de cargar la escena")]
    [SerializeField] private float delaySeconds = 5f;

    void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    private IEnumerator LoadAfterDelay()
    {
        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            yield break;
        }

        yield return new WaitForSeconds(Mathf.Max(0f, delaySeconds));

        // Carga asíncrona para evitar congelar el hilo principal
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        if (op == null)
        {
            yield break;
        }

        // Opcional: esperar a que termine la carga
        while (!op.isDone)
            yield return null;
    }
}