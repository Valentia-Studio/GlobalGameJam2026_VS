using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject quite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Empezar()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenuUI.SetActive(true); // Show the pause menu UI
    }

    public void Continuar()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenuUI.SetActive(false); 
        NoSalir();

    }

    public void Salir()
    {
        Application.Quit(); // Quit the application
    }

    public void Quite()
    {
        quite.SetActive(true);
    }

    public void NoSalir()
    {
        quite.SetActive(false);
    }

    public void SceneLoad(string scene)
    {
        // carga la escen  específica
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
