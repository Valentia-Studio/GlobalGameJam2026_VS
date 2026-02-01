using UnityEngine;

public class Move_Section : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(0, 0, SectionManager.Instance.currentSpeed) * Time.deltaTime;
    }
}