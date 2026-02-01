using UnityEngine;
using DG.Tweening;

public class BounceUI : MonoBehaviour
{
    public float maxScale = 1.2f;   // Escala máxima
    public float duration = 0.5f;   // Tiempo del efecto

    public Ease easeType = Ease.InBounce;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        transform.DOScale(originalScale * maxScale, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
