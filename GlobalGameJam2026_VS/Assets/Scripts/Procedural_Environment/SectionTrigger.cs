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
                // Verificar si es la sección inicial
                bool isInitialSection = Vector3.Distance(sectionData.transform.position,
                    Vector3.zero) < 0.1f;

                if (!isInitialSection)
                {
                    if (GameManager.Instance.currentState == GameManager.GameState.OnTrack)
                    {
                        SectionManager.Instance.SpawnNextSection();
                    }
                    Destroy(sectionData.gameObject, 1f);
                }
            }
        }

        if (other.gameObject.CompareTag("Station_Stop"))
        {
            GameManager.Instance.StopAtStation();
        }
    }
}