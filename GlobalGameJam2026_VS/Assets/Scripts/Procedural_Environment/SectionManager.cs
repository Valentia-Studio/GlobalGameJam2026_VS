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

    private void Awake()
    {
        Instance = this;
        currentSpeed = sectionSpeed;
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
        //Smooth para parar el tren
        float targetSpeed = isStopped ? 0f : sectionSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / stopSmoothTime);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnStation();
            timer = stationSpawnTime;
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