using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    [SerializeField] AudioClip deathAudio;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            GameManager.instance.Death(collision.gameObject, deathAudio);

            //SceneManager.LoadScene(actualSceneName);
        }
    }
}
