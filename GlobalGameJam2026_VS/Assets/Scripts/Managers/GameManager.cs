using UnityEngine;
using TMPro;

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
    private bool isFullyStopped = false;

    [Header("UI")]
    [SerializeField] TMP_Text passengersBoarding_Txt;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        passengersBoarding_Txt.enabled = false;
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

    public void StartGame()
    {
        currentState = GameState.OnTrack;
        SectionManager.Instance.StartMovement();
    }

    private void ResumeGame()
    {
        currentState = GameState.OnTrack;
        isFullyStopped = false;
        SectionManager.Instance.ResumeMovement();
    }

    public void StopAtStation()
    {
        currentState = GameState.OnStopped;
        SectionManager.Instance.StopAllSections();
        Invoke(nameof(SetFullyStopped), SectionManager.Instance.stopSmoothTime);

        passengersBoarding_Txt.enabled = true;

    }

    private void SetFullyStopped()
    {
        isFullyStopped = true;
    }
}