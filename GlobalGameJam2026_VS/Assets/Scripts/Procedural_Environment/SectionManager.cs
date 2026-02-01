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
    private bool canSpawnStations = true;
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
        SpawnInitialSections();
    }

    private void SpawnInitialSections()
    {
        GameObject firstSection = Instantiate(GetRandomSection(), Vector3.zero, Quaternion.identity);
        lastSection = firstSection.GetComponent<Section>();

        if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
        {
            Vector3 currentSpawnPos = lastSection.nextSection_SpawnPosition.position;

            for (int i = 1; i < initialSectionsCount; i++)
            {
                GameObject newSection = Instantiate(GetRandomSection(), currentSpawnPos, Quaternion.identity);
                Section sectionComp = newSection.GetComponent<Section>();

                if (sectionComp != null && sectionComp.nextSection_SpawnPosition != null)
                {
                    currentSpawnPos = sectionComp.nextSection_SpawnPosition.position;
                    lastSection = sectionComp;
                }
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

            if (timer <= 0f && !stationSpawned && canSpawnStations)
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

    public void StopSpawningStations()
    {
        canSpawnStations = false;
    }

    public void ResetForNewLevel()
    {
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
        canSpawnStations = true;

        SpawnInitialSections();
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