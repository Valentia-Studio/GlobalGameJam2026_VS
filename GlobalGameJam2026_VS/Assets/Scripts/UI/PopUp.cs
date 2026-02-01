using UnityEngine;
using DG.Tweening;

public class PopUp : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        transform.localScale = Vector3.zero;

        transform.DOScale(originalScale, duration)
            .SetEase(easeType)
            .SetUpdate(true); // Para que funcione aunque el juego esté pausado
    }
}