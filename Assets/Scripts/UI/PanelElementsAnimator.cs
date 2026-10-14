using DG.Tweening;
using UnityEngine;

public class PanelElementsAnimator : MonoBehaviour
{
    public RectTransform[] elements;   // título, botones de nivel, botón Volver
    public float duration = 0.35f;
    public float stagger = 0.08f;

    Vector3[] _scale;

    void Awake()
    {
        _scale = new Vector3[elements.Length];
        for (int i = 0; i < elements.Length; i++) _scale[i] = elements[i].localScale;
    }

    public void HideInstant()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].DOKill();
            elements[i].localScale = Vector3.zero;
        }
    }

    public Tween PlayIn()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < elements.Length; i++)
            seq.Insert(i * stagger,
                elements[i].DOScale(_scale[i], duration).SetEase(Ease.OutBack));
        return seq;
    }

    public Tween PlayOut()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < elements.Length; i++)
            seq.Insert(i * stagger * 0.5f,
                elements[i].DOScale(Vector3.zero, duration * 0.7f).SetEase(Ease.InBack));
        return seq;
    }
}