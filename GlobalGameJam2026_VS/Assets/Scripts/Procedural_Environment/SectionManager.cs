using UnityEngine;
using TMPro;

public class SectionManager : MonoBehaviour
{
    public static SectionManager Instance;

    [SerializeField] GameObject[] roadSections;
    [SerializeField] GameObject stationPrefab;
    [SerializeField] Transform firstSpawnPosition;
    [SerializeField] int initialSectionsCount = 3;
    [SerializeField] float stationSpawnTime = 30f;
    [SerializeField] float sectionSpeed = -5f;
    public float stopSmoothTime = 2f;

    [HideInInspector] bool isStopped = false;
    public float currentSpeed;

    private Section lastSection;
    private float timer;
    private bool stationSpawned = false;
    private float smoothTimer = 0f;
    private float startSpeed;
    private float targetSpeed;
    private void Awake()
    {
        Instance = this;
        currentSpeed = 0f;
        isStopped = true;
        startSpeed = 0f;
        targetSpeed = 0f;
    }

    private void Start()
    {
        Transform spawnPoint = firstSpawnPosition;

        for (int i = 0; i < initialSectionsCount; i++)
        {
            GameObject newSection = Instantiate(GetRandomSection(), spawnPoint.position, Quaternion.identity);
            lastSection = newSection.GetComponent<Section>();

            if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
            {
                spawnPoint = lastSection.nextSection_SpawnPosition;
            }
        }

        timer = stationSpawnTime;
    }

    private void Update()
    {
        float newTargetSpeed = isStopped ? 0f : sectionSpeed;

        if (newTargetSpeed != targetSpeed)
        {
            startSpeed = currentSpeed;
            targetSpeed = newTargetSpeed;
            smoothTimer = 0f;
        }

        if (smoothTimer < stopSmoothTime)
        {
            smoothTimer += Time.deltaTime;
            float t = Mathf.Clamp01(smoothTimer / stopSmoothTime);
            currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        if (GameManager.Instance != null && GameManager.Instance.currentState == GameManager.GameState.OnTrack)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && !stationSpawned)
            {
                SpawnStation();
                stationSpawned = true;
            }
        }
    }

    public void StartMovement()
    {
        isStopped = false;
    }

    public void ResumeMovement()
    {
        isStopped = false;
        timer = stationSpawnTime;
        stationSpawned = false;
    }

    public void SpawnNextSection()
    {
        if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
        {
            GameObject newSection = Instantiate(GetRandomSection(), lastSection.nextSection_SpawnPosition.position, Quaternion.identity);
            lastSection = newSection.GetComponent<Section>();
        }
    }

    private void SpawnStation()
    {
        if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
        {
            GameObject newStation = Instantiate(stationPrefab, lastSection.nextSection_SpawnPosition.position, Quaternion.identity);
            lastSection = newStation.GetComponent<Section>();
        }
    }

    public void StopAllSections()
    {
        isStopped = true;
    }

    private GameObject GetRandomSection()
    {
        return roadSections[Random.Range(0, roadSections.Length)];
    }
}