using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Tooltip("Lista de cámaras a activar/desactivar en orden (Cam1, Cam2, Cam3, Cam4)")]
    public GameObject[] cameras;

    [Tooltip("Tiempo mínimo entre cambios (segundos)")]
    public float switchCooldown = 2f;

    private int index = 0;
    private float lastSwitchTime = -999f;

    void Start()
    {
        ApplyCurrent();
        lastSwitchTime = Time.time - switchCooldown; // permitir el primer cambio inmediatamente
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // click derecho
        {
            TryNext();
        }
    }

    private void TryNext()
    {
        if (cameras == null || cameras.Length == 0) return;
        if (Time.time - lastSwitchTime < switchCooldown) return; // aún en cooldown

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
