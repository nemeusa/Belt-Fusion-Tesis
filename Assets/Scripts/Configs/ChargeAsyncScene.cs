using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChargeAsyncScene : MonoBehaviour
{
    [SerializeField] Image _loaderImage;
    [SerializeField] GameObject _loaderPanel, _buttonLoader;

    AsyncOperation _asyncOp;

    private void Start()
    {
        _loaderImage.fillAmount = 0;
        _loaderPanel.SetActive(false);
        _buttonLoader.SetActive(false);
    }
    public void ChangeScene(string sceneName) => StartCoroutine(ChangingMyScene(sceneName));

    IEnumerator ChangingMyScene(string sceneName)
    {
        //_asyncOp = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
        _asyncOp = SceneManager.LoadSceneAsync(sceneName);
        _loaderPanel.SetActive(true);
        StartMyScene(false); //Opcional de clase

        //Application.backgroundLoadingPriority = ThreadPriority.High; //Mas prioridad a carga asincrona que al juego.
        //Application.backgroundLoadingPriority = ThreadPriority.Normal; 
        //Application.backgroundLoadingPriority = ThreadPriority.BelowNormal; 
        Application.backgroundLoadingPriority = ThreadPriority.Low; //Mas prioridad al juego que a la carga asincrona.

        while (!_asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(_asyncOp.progress / 0.9f);
            yield return new WaitForEndOfFrame();
            _loaderImage.fillAmount = Mathf.MoveTowards(_loaderImage.fillAmount, progress, Time.deltaTime);
            if(_loaderImage.fillAmount >= 1) _buttonLoader.SetActive(true); //Opcional de clase
        }
    }

    public void StartMyScene(bool value) => _asyncOp.allowSceneActivation = value;
}
