using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject quite;
    [SerializeField] private GameObject options;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
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
        NonOptions();

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




    public void Options()
    {
        options.SetActive(true);
    }

    public void NonOptions()
    {
        options.SetActive(false);
    }



    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reanuda el tiempo
        isPaused = false;
        Continuar();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pausa el tiempo
        isPaused = true;
    }

}
