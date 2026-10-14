using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PortalElement { Neutral, Fire, Ice }

[Serializable]
public class ElementStyle
{
    public PortalElement element;
    public Color flashColor = Color.white;
    public float diveDuration = 0.8f;
    public Ease diveEase = Ease.InCubic;
    public float emergeDuration = 0.9f;
    public Ease emergeEase = Ease.OutCubic;
}

public class MenuTransitioner : MonoBehaviour
{
    public MainMenu mainMenu;
    public Camera cam;
    public Image flash;
    public ElementStyle[] styles;

    [Header("Ajustes")]
    public float uiFade = 0.2f;
    public float extraPastCamera = 1.5f;
    public float emergeDistance = 50f;
    [Range(0f, 1f)] public float aimAtCamera = 1f;

    [Header("Portal")]
    public bool portalFacesCamera = true;
    public Vector3 portalFacingEuler = Vector3.zero;   // si se ve de espaldas, probá (0,180,0)
    [Range(0.2f, 1f)] public float rotateUntil = 0.6f; // a qué altura del recorrido termina de girar

    [Header("Atravesar")]
    public Ease approachEase = Ease.InOutQuad;        // curva propia del acercamiento
    [Range(0f, 1f)] public float pastFactor = 0.3f;   // cuánto se pasa de largo la cámara (30% extra)
    [Range(0.3f, 0.95f)] public float flashStart = 0.7f; // cuándo empieza a taparse la pantalla

    public bool IsBusy { get; private set; }

    ElementStyle GetStyle(PortalElement e)
    {
        foreach (var s in styles) if (s.element == e) return s;
        return styles.Length > 0 ? styles[0] : new ElementStyle();
    }

    public void Go(PortalPanel from, PortalPanel to, bool back, GameObject selectAfter)
    {
        if (IsBusy) return;
        IsBusy = true;
        mainMenu.inputLocked = true;
        EventSystem.current.SetSelectedGameObject(null);

        ElementStyle st = GetStyle(back ? from.element : to.element);
        from.SaveRest();
        to.SaveRest();

        // Diagnóstico: el portal tiene que colgar del worldRoot
        bool portalIsChild = from.portal.IsChildOf(from.worldRoot);
        if (!portalIsChild)
            Debug.LogWarning("[MenuTransitioner] El portal NO es hijo de worldRoot en '" + from.name +
                             "'. Lo muevo por separado, pero conviene meterlo adentro.", from);

        Vector3 camPos = cam.transform.position;
        Vector3 P0 = from.portal.position;
        Vector3 W0 = from.worldRoot.position;
        Quaternion Wr0 = from.worldRoot.rotation;
        Quaternion portalRot0 = from.portal.rotation;

        // Desplazamiento: en línea recta del portal a la cámara, y un poco más allá
        Vector3 toCam = camPos - P0;
        Debug.Log("Distancia portal-cámara: " + toCam.magnitude.ToString("F1"));
        Vector3 dirToCam = toCam.normalized;
        Vector3 disp = back ? -dirToCam * emergeDistance
                    : toCam * (1f + pastFactor);

        // Giro: solo en Y, hasta quedar de frente a la cámara
        bool rotate = portalFacesCamera && !back;
        Vector3 flat = -cam.transform.forward;
        flat.y = 0f;
        if (flat.sqrMagnitude < 0.001f) flat = Vector3.back;
        Quaternion portalRot1 = Quaternion.LookRotation(flat.normalized, Vector3.up)
                                * Quaternion.Euler(portalFacingEuler);
        Quaternion Rfull = rotate ? portalRot1 * Quaternion.Inverse(portalRot0) : Quaternion.identity;

        // Personajes que salen corriendo (opcional)
        int n = (!back && from.runners != null) ? from.runners.Length : 0;
        Vector3[] run0 = new Vector3[n];
        Vector3[] runDir = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            Transform r = from.runners[i];
            run0[i] = r.localPosition;
            runDir[i] = r.parent.InverseTransformDirection(r.forward).normalized * from.runSign;
        }

        Color c = st.flashColor; c.a = 0f;
        flash.color = c;

        Sequence outSeq = DOTween.Sequence();
        outSeq.SetUpdate(UpdateType.Late);
        if (from.ui != null) outSeq.Insert(0f, from.ui.DOFade(0f, uiFade));

        outSeq.Insert(0f, DOVirtual.Float(0f, 1f, st.diveDuration, t =>
        {
            Vector3 P = P0 + disp * t;
            float g = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / rotateUntil));
            Quaternion Rd = Quaternion.Slerp(Quaternion.identity, Rfull, g);

            // Todo el mundo gira alrededor del portal y viaja con él
            from.worldRoot.SetPositionAndRotation(P + Rd * (W0 - P0), Rd * Wr0);
            if (!portalIsChild) from.portal.SetPositionAndRotation(P, Rd * portalRot0);

            // Los personajes, además, salen corriendo hacia adelante
            for (int i = 0; i < n; i++)
                from.runners[i].localPosition = run0[i] + runDir[i] * (from.runDistance * t);
        }).SetEase(back ? Ease.InQuad : approachEase));

        outSeq.Insert(st.diveDuration * flashStart, flash.DOFade(1f, st.diveDuration * (1f - flashStart)));
        outSeq.OnComplete(() =>
        {
            for (int i = 0; i < n; i++) from.runners[i].localPosition = run0[i];
            from.worldRoot.rotation = from.restRot;
            if (!portalIsChild) from.portal.rotation = portalRot0;
            Emerge(from, to, back, st, selectAfter);
        });
    }

    void Emerge(PortalPanel from, PortalPanel to, bool back, ElementStyle st, GameObject selectAfter)
    {
        from.gameObject.SetActive(false);
        from.worldRoot.position = from.restPos;
        if (from.ui != null) from.ui.alpha = 1f;

        to.gameObject.SetActive(true);
        Vector3 axis = to.Axis;
        Vector3 toCam = cam.transform.position - to.portal.position;
        float along = Mathf.Max(Vector3.Dot(toCam, axis), 5f);

        to.worldRoot.position = back
            ? to.restPos + axis * (along * 0.8f)
            : to.restPos - axis * emergeDistance;
        if (to.ui != null) to.ui.alpha = 0f;

        Sequence inSeq = DOTween.Sequence();
        inSeq.Insert(0f, to.worldRoot.DOMove(to.restPos, st.emergeDuration).SetEase(st.emergeEase));
        inSeq.Insert(0f, flash.DOFade(0f, st.emergeDuration * 0.5f));
        if (to.ui != null) inSeq.Insert(st.emergeDuration * 0.5f, to.ui.DOFade(1f, uiFade * 2f));
        inSeq.OnComplete(() =>
        {
            IsBusy = false;
            mainMenu.inputLocked = false;
            EventSystem.current.SetSelectedGameObject(selectAfter);
        });
    }
}