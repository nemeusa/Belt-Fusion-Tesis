using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public static string _nextLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _nextLevel = nextLevel;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            SceneManager.LoadScene(_nextLevel);
    }


}
