using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject levelCompletePanel;
    public GameObject gameCompletePanel;
    public GameObject startButtonPanel;
    public TMP_Text levelCompleteText;
    public Button startButton;
    public Button restartButtonGameComplete;

    private void Awake()
    {
        Instance = this;
        HideAllPanels();

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        if (restartButtonGameComplete != null)
        {
            restartButtonGameComplete.onClick.AddListener(OnRestartButtonClicked);
        }
    }

    private void Update()
    {
        if (levelCompletePanel != null && levelCompletePanel.activeSelf && Input.GetKeyDown(KeyCode.N))
        {
            HideAllPanels();
            GameManager.Instance.LoadNextLevel();
        }
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

    public void ShowStartButton()
    {
        if (startButtonPanel != null)
        {
            startButtonPanel.SetActive(true);
        }
    }

    public void HideStartButton()
    {
        if (startButtonPanel != null)
        {
            startButtonPanel.SetActive(false);
        }
    }

    public void HideAllPanels()
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

    private void OnRestartButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            HideAllPanels();
            GameManager.Instance.RestartGame();
        }
    }
}