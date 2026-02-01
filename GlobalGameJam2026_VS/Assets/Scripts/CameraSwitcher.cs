using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject[] cameras;

    public float switchCooldown = 2f;

    private int index = 0;
    private float lastSwitchTime = -999f;

    void Start()
    {
        ApplyCurrent();
        lastSwitchTime = Time.time - switchCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TryNext();
        }
    }

    private void TryNext()
    {
        if (cameras == null || cameras.Length == 0) return;
        if (Time.time - lastSwitchTime < switchCooldown) return; 

        index = (index + 1) % cameras.Length;
        ApplyCurrent();
        lastSwitchTime = Time.time;
    }

    private void ApplyCurrent()
    {
        if (cameras == null || cameras.Length == 0) return;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
                cameras[i].SetActive(i == index);
        }
    }
}
