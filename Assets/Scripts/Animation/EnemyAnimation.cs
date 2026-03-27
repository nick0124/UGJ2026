using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Enemy))]
public class EnemyAnimation : MonoBehaviour
{
    [Header("Настройки тряски")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.5f;
    [SerializeField] private int shakeVibrato = 10;

    [Header("Настройки отталкивания (Knockback)")]
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Space(20f)]

    [Header("Парение (Hover)")]
    public float hoverHeight = 1.5f;      // Высота подъема
    public float hoverDuration = 2f;      // Время цикла

    private Color originalColor;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.onTakeDamage += PlayDamageEffect;

        WalkAnimation();
    }

    private void WalkAnimation()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * hoverHeight;

        // Создаем последовательность: Вверх -> Вниз
        Sequence seq = DOTween.Sequence();

        // Движение вверх
        seq.Append(transform.DOMoveY(endPos.y, hoverDuration)
            .SetEase(Ease.InOutSine)); // Плавное ускорение и замедление

        // Движение вниз
        seq.Append(transform.DOMoveY(startPos.y, hoverDuration)
            .SetEase(Ease.InOutSine));

        // Зацикливаем бесконечно
        seq.SetLoops(-1, LoopType.Yoyo)
           .SetTarget(this); // Важно для очистки через DOTween.Kill(this)
    }

    public void PlayDamageEffect()
    {
        // 1. Останавливаем предыдущие анимации урона, чтобы они не накладывались
        DOTween.Kill(this);

        // 2. Создаем последовательность эффектов
        Sequence damageSeq = DOTween.Sequence();

        // --- ЭФФЕКТ 2: Тряска позиции ---
        // Запускаем одновременно с вспышкой (Join)
        damageSeq.Join(transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, 90, false, true));

        // --- ЭФФЕКТ 3: Отталкивание (Knockback) ---
        // Сдвигаем врага назад от удара и возвращаем
        Vector3 knockbackPos = transform.position + (-transform.forward.normalized * knockbackForce);

        // Быстрый отлет
        damageSeq.Prepend(transform.DOMove(knockbackPos, knockbackDuration).SetEase(Ease.OutQuad));
        // Возврат на место (если нужно, чтобы враг не смещался навсегда)
        // Если враг патрулирует, лучше не возвращать позицию жестко, а дать физике или ИИ вернуть его
        damageSeq.Append(transform.DOMove(transform.position, knockbackDuration).SetEase(Ease.InQuad));

        // Привязываем к этому объекту для безопасной очистки
        damageSeq.SetTarget(this);
    }
}
