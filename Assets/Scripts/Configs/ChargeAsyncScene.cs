using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChargeAsyncScene : MonoBehaviour
{
    [SerializeField] Image _loaderImage;
    [SerializeField] GameObject _loaderPanel, _buttonLoader;


    [SerializeField] Animator _ani;
    [SerializeField] GameObject ActivebackgroundRosa;

    [Header("Silueta del personaje")]
    [SerializeField] RectTransform _silhouette;
    [SerializeField] float _offscreenX = 1500f; // que quede fuera de la pantalla a cada lado
    [SerializeField] float _enterTime = 0.8f;
    [SerializeField] float _exitTime = 0.6f;


    AsyncOperation _asyncOp;
    bool isLoading;

    private void Start()
    {
        _loaderImage.fillAmount = 0;
        _loaderPanel.SetActive(false);
        _buttonLoader.SetActive(false);

        _silhouette.anchoredPosition = new Vector2(-_offscreenX, _silhouette.anchoredPosition.y);

    }
    public void ChangeScene(string sceneName) => StartCoroutine(ChangingMyScene(sceneName));

    IEnumerator ChangingMyScene(string sceneName)
    {
        if (isLoading) yield break;
        isLoading = true;

        _asyncOp = SceneManager.LoadSceneAsync(sceneName);
        _asyncOp.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        _loaderPanel.SetActive(true);

        // 1) Entra desde la izquierda hasta el medio
        Tween enter = _silhouette.DOAnchorPosX(0f, _enterTime).SetEase(Ease.OutCubic);

        // 2) Espera a que cargue (y a que haya terminado de entrar)
        while (_loaderImage.fillAmount < 1f || (enter.IsActive() && enter.IsPlaying()))
        {
            float progress = Mathf.Clamp01(_asyncOp.progress / 0.9f);
            _loaderImage.fillAmount = Mathf.MoveTowards(_loaderImage.fillAmount, progress, Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        _buttonLoader.SetActive(true);
        _ani.SetBool("Next", true);
        ActivebackgroundRosa.SetActive(true);
        yield return new WaitForSeconds(1f);

        // 3) Se va por la derecha y recién ahí activa la escena
        yield return _silhouette.DOAnchorPosX(_offscreenX, _exitTime)
                                .SetEase(Ease.InCubic)
                                .WaitForCompletion();

        _asyncOp.allowSceneActivation = true;
        while (!_asyncOp.isDone) yield return null;
    }

    public void StartMyScene(bool value) => _asyncOp.allowSceneActivation = value;
}
