using UnityEngine;

public class BreakableIce : MonoBehaviour, ISlamReactive
{
    public void OnSlam(PlayerController player) { Destroy(gameObject); }
}