using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    [SerializeField] float time;
    void Start()
    {
        Destroy(gameObject, time);
    }

}
