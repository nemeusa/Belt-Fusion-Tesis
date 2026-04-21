using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevel : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            GameManager.instance.WinGame();
            Destroy(gameObject);
        }
    }

}
