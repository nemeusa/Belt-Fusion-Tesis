using UnityEngine;

public class FireBallVolcan : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
