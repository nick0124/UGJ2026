using DG.Tweening;
using UnityEngine;

public class DropAnimation : MonoBehaviour
{
    [Header("Настройки падения")]
    public float dropDuration = 0.4f;       // Время падения
    public float bounceHeight = 0.5f;       // Высота отскока
    public float bounceDuration = 0.3f;     // Время отскока

    [Header("Вращение")]
    public float rotationSpeed = 100f;      // Скорость вращения в покое
    public Vector3 rotationAxis = Vector3.up;

    private Sequence _dropSequence;

    private void Awake()
    {
        Drop();
    }


    // Инициализация и запуск анимации дропа
    public void Drop()
    {
        // 1. Анимация появления (Pop In)
        Tween scaleIn = transform.DOScale(30f, 0.2f).SetEase(Ease.OutBack);

        // 2. Анимация падения и отскока
        _dropSequence = DOTween.Sequence();

        // Падение вниз (ускорение как у гравитации)
        _dropSequence.Append(transform.DOMoveY(1f, dropDuration).SetEase(Ease.InQuad));

        // Отскок вверх
        _dropSequence.Append(transform.DOMoveY(1f + bounceHeight, bounceDuration).SetEase(Ease.OutQuad));

        // Возврат на землю
        _dropSequence.Append(transform.DOMoveY(1f, bounceDuration).SetEase(Ease.InQuad));

        // 3. Запуск вращения после приземления
        _dropSequence.OnComplete(() =>
        {
            // Небольшой "сплющивание" при ударе о землю для эффекта веса
            transform.DOScale(new Vector3(30f, 20f, 30f), 0.1f).OnComplete(() =>
            {
                transform.DOScale(25f, 0.1f);
            });
        });

        _dropSequence.SetTarget(this);

        transform
            .DORotate(new Vector3(0f, 360f, 0f), rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
