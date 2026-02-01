using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("New_Section"))
        {
            Section sectionData = other.GetComponentInParent<Section>();
            if (sectionData != null)
            {
                if (GameManager.Instance.currentState == GameManager.GameState.OnTrack)
                {
                    SectionManager.Instance.SpawnNextSection();
                }
                Destroy(sectionData.gameObject, 1f);
            }
        }

        if (other.gameObject.CompareTag("Station_Stop"))
        {
            GameManager.Instance.StopAtStation();

            NPCSpawner spawner = FindNPCSpawnerInStation(other.transform);
            if (spawner != null)
            {
                spawner.StartSpawning();
            }
        }
    }

    private NPCSpawner FindNPCSpawnerInStation(Transform stationTransform)
    {
        NPCSpawner spawner = stationTransform.GetComponentInParent<NPCSpawner>();

        if (spawner == null)
        {
            spawner = stationTransform.GetComponentInChildren<NPCSpawner>();
        }

        if (spawner == null)
        {
            Transform parent = stationTransform;
            while (parent != null)
            {
                spawner = parent.GetComponent<NPCSpawner>();
                if (spawner != null) break;
                parent = parent.parent;
            }
        }

        return spawner;
    }
}