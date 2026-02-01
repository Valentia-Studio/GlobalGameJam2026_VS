using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public static SectionManager Instance;

    public GameObject[] roadSections;
    public GameObject stationPrefab;
    public Transform firstSpawnPosition;
    public int initialSectionsCount = 3;
    public float stationSpawnTime = 30f;
    public float sectionSpeed = -5f;
    public float stopSmoothTime = 2f;

    [HideInInspector] public bool isStopped = false;
    [HideInInspector] public float currentSpeed;

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

    public void ResetForNewLevel()
    {
        print("New Level");
        if (firstSpawnPosition == null) return;

        Vector3 savedSpawnPosition = firstSpawnPosition.position;
        Quaternion savedSpawnRotation = firstSpawnPosition.rotation;

        Section[] allSections = FindObjectsByType<Section>(FindObjectsSortMode.None);
        foreach (Section section in allSections)
        {
            Destroy(section.gameObject);
        }

        lastSection = null;
        currentSpeed = 0f;
        isStopped = true;
        startSpeed = 0f;
        targetSpeed = 0f;
        smoothTimer = 0f;
        timer = stationSpawnTime;
        stationSpawned = false;

        Invoke(nameof(DelayedSpawnInitialSections), 0.1f);
    }

    private void DelayedSpawnInitialSections()
    {
        if (firstSpawnPosition == null) return;

        Transform spawnPoint = firstSpawnPosition;

        for (int i = 0; i < initialSectionsCount; i++)
        {
            GameObject newSection = Instantiate(GetRandomSection(), spawnPoint.position, Quaternion.identity);
            Section sectionComponent = newSection.GetComponent<Section>();

            if (sectionComponent != null && sectionComponent.nextSection_SpawnPosition != null)
            {
                spawnPoint = sectionComponent.nextSection_SpawnPosition;
                lastSection = sectionComponent;
            }
        }
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