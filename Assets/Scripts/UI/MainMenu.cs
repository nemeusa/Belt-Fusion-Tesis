using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject primerBoton;

    public AudioMixer mixer;

    bool isMuted = false;


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(primerBoton);
        }
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Mute(string nameG)
    {
        isMuted = !isMuted;

        if (isMuted)
            mixer.SetFloat(nameG, -80);
        else
            mixer.SetFloat(nameG, 0);
    }


    public void OffIcon(GameObject offIcon)
    {
        if(isMuted)
            offIcon.SetActive(true);

        else
            offIcon.SetActive(false);
    }

}
