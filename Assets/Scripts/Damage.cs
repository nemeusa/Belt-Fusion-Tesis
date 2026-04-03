using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManager.LoadScene("PadreScene");
        }
    }
}
