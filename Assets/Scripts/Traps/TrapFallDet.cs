using UnityEngine;

public class TrapFallDet : MonoBehaviour
{
    public TrapFall platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && !platform.falling)
        {
            Debug.Log("fall");
            StartCoroutine(platform.DownFall());
        }
    }
}
