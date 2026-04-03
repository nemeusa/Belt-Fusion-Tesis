using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 6;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.position += speed * Vector3.down * Time.deltaTime;  
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }
}
