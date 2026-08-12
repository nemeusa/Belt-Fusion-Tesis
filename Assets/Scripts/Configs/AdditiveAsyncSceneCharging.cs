using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveAsyncSceneCharging : MonoBehaviour
{
    [SerializeField] string _sceneToAddName;
    [SerializeField] string _sceneToRemoveName;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(_sceneToAddName, LoadSceneMode.Additive);
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            if (_sceneToRemoveName != "") SceneManager.UnloadSceneAsync(_sceneToRemoveName);
            gameObject.SetActive(false);
        }
    }

    //Traer Canvas del Player y Joystick de la clase 2
}
