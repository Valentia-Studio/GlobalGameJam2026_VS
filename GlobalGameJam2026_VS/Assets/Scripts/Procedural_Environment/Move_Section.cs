using UnityEngine;
using UnityEditor;

public class Move_Section : MonoBehaviour
{
    [SerializeField] float sectionSpeed;
    private void Update()
    {
        transform.position += new Vector3(0, 0, sectionSpeed) * Time.deltaTime;
    }
}
