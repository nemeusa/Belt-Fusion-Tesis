using PopupFramework.Core;
using PopupFramework.Demo.Confirmation;
using PopupFramework.Demo.Rewards;
using PopupFramework.Popups;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivePopup : MonoBehaviour
{
    [SerializeField] string title;
    [SerializeField] string message;
    [SerializeField] string details;
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject selectLastButton;
    [SerializeField] GameObject selectFirstButton;

    [SerializeField] bool exitGame;

    public void ShowConfirmation()
    {
        if (!ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
        {
            Debug.LogWarning("IPopupQueue not found in the scene yet.");
            return;
        }



        queue.Show(new ConfirmationPopupData
        {
            title = this.title,
            message = this.message,
            yesLabel = "yes",
            noLabel = "no",
            onYes = () => {
                if (!exitGame) SceneManager.LoadScene(nextSceneName);
                else ExitGame();
                },
            onNo = () =>
            {
                EventSystem.current.SetSelectedGameObject(selectLastButton);

                Debug.Log("Kept editing");
            },
        });
            EventSystem.current.SetSelectedGameObject(selectFirstButton);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
