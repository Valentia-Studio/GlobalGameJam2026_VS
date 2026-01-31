using Unity.VisualScripting;
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
                SectionManager.Instance.SpawnNextSection();
                Destroy(sectionData.gameObject, 1f);
            }
        }

        if (other.gameObject.CompareTag("Station_Stop"))
        {
            SectionManager.Instance.StopAllSections();
        }

    }
}