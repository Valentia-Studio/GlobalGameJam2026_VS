using UnityEngine;
using TMPro;
using UnityEngine.UI; // Añadir este using

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject levelCompletePanel;
    public GameObject gameCompletePanel;
    public GameObject startButtonPanel; // Nuevo panel para el botón de inicio
    public TMP_Text levelCompleteText;
    public Button startButton; // Referencia al botón de inicio

    private void Awake()
    {
        Instance = this;
        HideAllPanels();

        // Configurar el listener del botón
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    private void Update()
    {
        if (levelCompletePanel != null && levelCompletePanel.activeSelf && Input.GetKeyDown(KeyCode.N))
        {
            HideAllPanels();
            GameManager.Instance.LoadNextLevel();
        }

        // Eliminar la verificación de la tecla E aquí
        // El juego ahora se inicia con el botón
    }

    public void ShowLevelComplete(int levelCompleted)
    {
        levelCompletePanel.SetActive(true);
        if (levelCompleteText != null)
        {
            levelCompleteText.text = "Level " + levelCompleted + " Completed";
        }
    }

    public void ShowGameComplete()
    {
        gameCompletePanel.SetActive(true);
    }

    public void ShowStartButton() // Nuevo método
    {
        if (startButtonPanel != null)
        {
            startButtonPanel.SetActive(true);
        }
    }

    public void HideStartButton() // Nuevo método
    {
        if (startButtonPanel != null)
        {
            startButtonPanel.SetActive(false);
        }
    }

    private void HideAllPanels()
    {
        levelCompletePanel.SetActive(false);
        gameCompletePanel.SetActive(false);
        HideStartButton();
    }

    private void OnStartButtonClicked()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.currentState == GameManager.GameState.InitialState)
        {
            HideStartButton();
            GameManager.Instance.StartGame();
        }
    }
}