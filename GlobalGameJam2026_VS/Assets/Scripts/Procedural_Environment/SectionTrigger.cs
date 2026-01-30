using Unity.VisualScripting;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;

    public Transform sectionPivot;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("New_Section"))
        {
            Instantiate(roadSection, new Vector3 (0, 0, -52.54176f), Quaternion.identity);
        }
    }
}
