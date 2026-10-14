using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPortalPreview : MonoBehaviour
{
    [System.Serializable]
    public class LevelEntry
    {
        public GameObject button;    // el botón del nivel
        public Material material;    // el material que le corresponde al portal
    }

    public Renderer portalQuad;      // el quad del portal de ESTE panel
    public LevelEntry[] levels;
    public float swapDuration = 0.3f;
    [Range(0.5f, 1f)] public float squash = 0.85f;

    Vector3 _baseScale;
    int _current = -1;
    Tween _tween;

    void Awake() { _baseScale = portalQuad.transform.localScale; }

    void OnEnable()
    {
        _tween?.Kill();
        portalQuad.transform.localScale = _baseScale;
        if (levels.Length > 0) Apply(0, true);   // arranca con el material del nivel 1
    }

    void OnDisable()
    {
        _tween?.Kill();
        portalQuad.transform.localScale = _baseScale;
    }

    void Update()
    {
        if (EventSystem.current == null) return;
        GameObject sel = EventSystem.current.currentSelectedGameObject;
        if (sel == null) return;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].button == sel)
            {
                if (i != _current) Apply(i, false);
                break;
            }
        }
        // Si está seleccionado otro botón (ej. Volver), se mantiene el último material
    }

    void Apply(int i, bool instant)
    {
        _current = i;
        Material m = levels[i].material;
        if (m == null) return;

        if (instant) { portalQuad.sharedMaterial = m; return; }

        _tween?.Kill();
        Transform t = portalQuad.transform;
        t.localScale = _baseScale;

        _tween = DOTween.Sequence()
            .Append(t.DOScale(_baseScale * squash, swapDuration * 0.4f).SetEase(Ease.InQuad))
            .AppendCallback(() => portalQuad.sharedMaterial = m)
            .Append(t.DOScale(_baseScale, swapDuration * 0.6f).SetEase(Ease.OutBack))
            .SetLink(gameObject);
    }
}