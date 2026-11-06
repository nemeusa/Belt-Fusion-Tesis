using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MandoMenuScroll : MonoBehaviour
{
    private ScrollRect _scroll;
    private GameObject _last;

    private void Awake() => _scroll = GetComponent<ScrollRect>();

    private void Update()
    {
        GameObject sel = EventSystem.current.currentSelectedGameObject;
        if (sel == null || sel == _last || !sel.transform.IsChildOf(_scroll.content))
            return;

        // Ignora los items de la lista desplegada de un dropdown
        if (sel.GetComponent<TMP_Dropdown>() == null && sel.GetComponentInParent<TMP_Dropdown>() != null)
            return;

        _last = sel;

        RectTransform content = _scroll.content;
        float scrollable = content.rect.height - _scroll.viewport.rect.height;
        if (scrollable <= 0f) return;

        RectTransform target = (RectTransform)sel.transform;
        Vector3 local = content.InverseTransformPoint(target.TransformPoint(target.rect.center));
        float fromTop = content.rect.yMax - local.y;
        float offset = Mathf.Clamp(fromTop - _scroll.viewport.rect.height * 0.5f, 0f, scrollable);

        _scroll.verticalNormalizedPosition = 1f - offset / scrollable;
    }
}
