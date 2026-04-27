using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public string actualSceneName;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            GameManager.instance.Death(collision.gameObject);

            //SceneManager.LoadScene(actualSceneName);
        }
    }
}
