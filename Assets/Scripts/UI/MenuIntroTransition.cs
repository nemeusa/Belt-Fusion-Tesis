using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuIntroTransition : MonoBehaviour
{
    [Header("Referencias")]
    public MainMenu mainMenu;
    public int menuPanelIndex = 1;
    public GameObject menuPanel;
    public SmallPortalSpawner spawner;

    [Header("Panel Título")]
    public RectTransform[] titleElements;      // los 2 PNG + Press X
    public Transform[] secondaryPortals;       // raíz de cada portal secundario
    public Transform[] portalWorlds;           // assets que están detrás de cada portal

    [Header("Portal principal")]
    public Transform mainPortalRoot;
    public Transform[] mainPortalSystems;
    public Transform menuPortalRoot;           // copia del panel Menú (solo referencia)
    public Transform[] menuPortalSystems;      // MISMO orden que mainPortalSystems

    [Header("Panel Menú")]
    public Transform worldRoot;                // Path + personajes
    public Vector3 worldStartOffset = new Vector3(0, 0, 40f); // tunear: "detrás" del portal
    public RectTransform[] menuButtons;

    [Header("Tiempos")]
    public float shrinkDuration = 0.4f;
    public float portalMoveDuration = 1f;
    public float pathDuration = 1.2f;

    Vector3 _targetPos;
    Quaternion _targetRot;
    Vector3[] _targetScales;
    Vector3 _worldEndPos;
    Vector2[] _buttonEndPos;
    bool _playing;

    void Awake()
    {
        // Guardamos la pose final ANTES de apagar la copia
        _targetPos = menuPortalRoot.position;
        _targetRot = menuPortalRoot.rotation;
        _targetScales = new Vector3[menuPortalSystems.Length];
        for (int i = 0; i < menuPortalSystems.Length; i++)
            _targetScales[i] = menuPortalSystems[i].localScale;

        _worldEndPos = worldRoot.position;
        _buttonEndPos = new Vector2[menuButtons.Length];
        for (int i = 0; i < menuButtons.Length; i++)
            _buttonEndPos[i] = menuButtons[i].anchoredPosition;
    }

    // Llamar desde el OnClick del botón "Press X"
    public void Play()
    {
        if (_playing) return;
        _playing = true;

        mainMenu.inputLocked = true;
        EventSystem.current.SetSelectedGameObject(null);

        // Preparar panel Menú
        menuPanel.SetActive(true);
        menuPortalRoot.gameObject.SetActive(false);   // la copia ya no hace falta
        worldRoot.position = _worldEndPos + worldStartOffset;
        worldRoot.gameObject.SetActive(false);        // se prende cuando el portal está listo
        for (int i = 0; i < menuButtons.Length; i++)
            menuButtons[i].anchoredPosition = _buttonEndPos[i] + Vector2.left * 700f;

        Sequence seq = DOTween.Sequence();

        // 1) Desaparecen título, Press X, portales secundarios y sus mundos
        float t = 0f;
        foreach (var rt in titleElements)
            seq.Insert(t, rt.DOScale(0f, shrinkDuration).SetEase(Ease.InBack));

        foreach (var p in secondaryPortals)
        {
            seq.Insert(t, p.DOScale(0f, shrinkDuration).SetEase(Ease.InBack));
            t += 0.06f; // escalonado
        }
        foreach (var w in portalWorlds)
            seq.Insert(0.1f, w.DOScale(0f, shrinkDuration).SetEase(Ease.InBack));

        // 2) Portal principal: se agranda, se mueve y rota a la pose del menú
        float portalStart = 0.35f;
        seq.Insert(portalStart, mainPortalRoot.DOMove(_targetPos, portalMoveDuration).SetEase(Ease.InOutCubic));
        seq.Insert(portalStart, mainPortalRoot.DORotateQuaternion(_targetRot, portalMoveDuration).SetEase(Ease.InOutCubic));
        for (int i = 0; i < mainPortalSystems.Length; i++)
            seq.Insert(portalStart, mainPortalSystems[i].DOScale(_targetScales[i], portalMoveDuration).SetEase(Ease.InOutCubic));

        // 3) El Path (con personajes) atraviesa el portal
        float pathStart = portalStart + portalMoveDuration * 0.7f;
        seq.InsertCallback(pathStart, () => worldRoot.gameObject.SetActive(true));
        seq.Insert(pathStart, worldRoot.DOMove(_worldEndPos, pathDuration).SetEase(Ease.OutCubic));

        // 4) Entran los botones, uno detrás del otro
        float btnStart = pathStart + pathDuration * 0.5f;
        for (int i = 0; i < menuButtons.Length; i++)
            seq.Insert(btnStart + i * 0.1f,
                menuButtons[i].DOAnchorPos(_buttonEndPos[i], 0.45f).SetEase(Ease.OutBack));

        // 5) Portales chicos de la derecha
        seq.InsertCallback(btnStart, () => spawner.StartSpawning());

        // Final: registramos el panel en el historial y devolvemos el control
        seq.OnComplete(() =>
        {
            mainPortalRoot.SetParent(worldRoot, true);          // el portal viaja con el Menú
            mainMenu.panels[0].panelObject.SetActive(false);    // apagamos el panel Título
            mainMenu.OpenPanelKeepingPrevious(menuPanelIndex);
            mainMenu.inputLocked = false;
        });
    }
}