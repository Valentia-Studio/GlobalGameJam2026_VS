using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        InitialState,
        OnTrack,
        OnStopped
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
    }

    private void Update()
    {
        if (currentState == GameState.InitialState && Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }

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

    private void StartGame()
    {
        currentState = GameState.OnTrack;
        SectionManager.Instance.StartMovement();
    }

    private void ResumeGame()
    {
        currentState = GameState.OnTrack;
        isFullyStopped = false;
        stationsVisited++;

        if (stationsVisited >= stationsPerLevel)
        {
            SectionManager.Instance.ResumeMovement();
            Invoke(nameof(EndLevel), levelEndDelay);
        }
        else
        {
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
        trainIndex++;
        stationsVisited = 0;
        currentState = GameState.InitialState;
        isFullyStopped = false;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOut(() => {
                SectionManager.Instance.ResetForNewLevel();
                SpawnTrain();
            });
        }
        else
        {
            SectionManager.Instance.ResetForNewLevel();
            SpawnTrain();
        }
    }

    private void ShowLevelComplete()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.ShowLevelComplete(currentLevel - 1);
        }
        else
        {
            Debug.Log("Level " + (currentLevel - 1) + " Complete! Press N to continue to next level");
        }
    }

    private void ShowGameComplete()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.ShowGameComplete();
        }
        else
        {
            Debug.Log("Game Complete! All levels finished!");
        }
    }
}