using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public static SectionManager Instance;

    public GameObject roadSection;
    public Transform firstSpawnPosition;
    public int initialSectionsCount = 3;

    private Section lastSection;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Transform spawnPoint = firstSpawnPosition;

        for (int i = 0; i < initialSectionsCount; i++)
        {
            GameObject newSection = Instantiate(roadSection, spawnPoint.position, Quaternion.identity);
            lastSection = newSection.GetComponent<Section>();

            if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
            {
                spawnPoint = lastSection.nextSection_SpawnPosition;
            }
        }
    }

    public void SpawnNextSection()
    {
        if (lastSection != null && lastSection.nextSection_SpawnPosition != null)
        {
            GameObject newSection = Instantiate(roadSection, lastSection.nextSection_SpawnPosition.position, Quaternion.identity);
            lastSection = newSection.GetComponent<Section>();
        }
    }
}
