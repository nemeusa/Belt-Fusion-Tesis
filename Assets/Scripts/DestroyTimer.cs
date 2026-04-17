using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] float destroyTime = 0.5f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
