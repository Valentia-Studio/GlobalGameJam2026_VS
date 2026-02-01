using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        InitialState,
        OnTrack,
        OnStopped,
        LevelEnding
    }

    public GameState currentState = GameState.InitialState;
    public GameObject[] trainPrefabs;
    public float levelEndDelay = 5f;

    private bool isFullyStopped = false;
    private int currentLevel = 1;
    private int stationsVisited = 0;
    private const int maxLevels = 3;
    private const int stationsPerLevel = 3;
    private GameObject currentTrain;
    private int[] trainOrder;
    private int trainIndex = 0;

    private void Awake()
    {
        Instance = this;
        GenerateTrainOrder();
        SpawnTrain();
        ShowStartButton();
    }

    private void Update()
    {
        if (currentState == GameState.OnStopped && isFullyStopped && Input.GetKeyDown(KeyCode.Q))
        {
            ResumeGame();
        }
    }

    private void GenerateTrainOrder()
    {
        trainOrder = new int[maxLevels];
        bool[] used = new bool[trainPrefabs.Length];

        for (int i = 0; i < maxLevels; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, trainPrefabs.Length);
            } while (used[randomIndex]);

            used[randomIndex] = true;
            trainOrder[i] = randomIndex;
        }
    }

    private void SpawnTrain()
    {
        if (currentTrain != null)
        {
            Destroy(currentTrain);
        }

        currentTrain = Instantiate(trainPrefabs[trainOrder[trainIndex]]);
    }

    public void StartGame()
    {
        currentState = GameState.OnTrack;
        SectionManager.Instance.StartMovement();
    }

    private void ResumeGame()
    {
        stationsVisited++;

        if (stationsVisited >= stationsPerLevel)
        {
            currentState = GameState.LevelEnding;
            isFullyStopped = false;
            SectionManager.Instance.ResumeMovement();
            SectionManager.Instance.StopSpawningStations();
            Invoke(nameof(EndLevel), levelEndDelay);
        }
        else
        {
            currentState = GameState.OnTrack;
            isFullyStopped = false;
            SectionManager.Instance.ResumeMovement();
        }
    }

    public void StopAtStation()
    {
        currentState = GameState.OnStopped;
        SectionManager.Instance.StopAllSections();
        Invoke(nameof(SetFullyStopped), SectionManager.Instance.stopSmoothTime);
    }

    private void SetFullyStopped()
    {
        isFullyStopped = true;
    }

    private void EndLevel()
    {
        SectionManager.Instance.StopAllSections();
        currentLevel++;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn(() => {
                if (currentLevel > maxLevels)
                {
                    ShowGameComplete();
                }
                else
                {
                    ShowLevelComplete();
                }
            });
        }
        else
        {
            if (currentLevel > maxLevels)
            {
                ShowGameComplete();
            }
            else
            {
                ShowLevelComplete();
            }
        }
    }

    public void LoadNextLevel()
    {
        ResetForNextLevel();

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOut(() => { });
        }
    }

    private void ResetForNextLevel()
    {
        trainIndex++;
        stationsVisited = 0;
        currentState = GameState.InitialState;
        isFullyStopped = false;

        SectionManager.Instance.ResetForNewLevel();
        SpawnTrain();

        ShowStartButton();
    }

    public void RestartGame()
    {
        currentLevel = 1;
        trainIndex = 0;
        stationsVisited = 0;
        currentState = GameState.InitialState;
        isFullyStopped = false;

        GenerateTrainOrder();

        if (currentTrain != null)
        {
            Destroy(currentTrain);
        }

        SectionManager.Instance.ResetForNewLevel();
        SpawnTrain();

        ShowStartButton();

        if (UIController.Instance != null)
        {
            UIController.Instance.HideAllPanels();
        }

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOut(() => { });
        }
    }

    private void ShowStartButton()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.ShowStartButton();
        }
    }

    private void ShowLevelComplete()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.ShowLevelComplete(currentLevel - 1);
        }
    }

    private void ShowGameComplete()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.ShowGameComplete();
        }
    }
}