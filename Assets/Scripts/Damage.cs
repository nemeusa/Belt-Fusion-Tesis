using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            GameManager.instance.Death(collision.gameObject);

            //SceneManager.LoadScene(actualSceneName);
        }
    }
}
