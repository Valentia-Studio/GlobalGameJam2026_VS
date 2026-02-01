using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public GameObject levelCompletePanel;
    public GameObject gameCompletePanel;
    public TMP_Text levelCompleteText;
    private void Awake()
    {
        Instance = this;
        HideAllPanels();
    }

    private void Update()
    {
        if (levelCompletePanel.activeSelf && Input.GetKeyDown(KeyCode.N))
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
            levelCompleteText.text = "Level " + levelCompleted + " Complete!\nPress N to continue";
        }
    }

    public void ShowGameComplete()
    {
        gameCompletePanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        levelCompletePanel.SetActive(false);
        gameCompletePanel.SetActive(false);
    }
}