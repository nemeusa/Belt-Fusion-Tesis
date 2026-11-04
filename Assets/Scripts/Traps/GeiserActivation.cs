using UnityEngine;

public class GeiserActivation : MonoBehaviour
{
    [SerializeField] Whale geyser;

    void OnTriggerEnter(Collider other) => Check(other);
    void OnTriggerStay(Collider other) => Check(other);

    void Check(Collider other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player != null && player.isSlamming)
            geyser.TryActivate();
    }
}
