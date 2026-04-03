using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeFire : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<FireBall>())
        {
            GameManager.instance.addPoints(1);
            Destroy(gameObject);
        }
    }
}
