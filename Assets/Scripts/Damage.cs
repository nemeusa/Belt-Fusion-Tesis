using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public string actualSceneName;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene(actualSceneName);
        }
    }
}
