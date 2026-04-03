using UnityEngine;

public class TakeElectricity : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().ElectricityTrail.emitting)
        {
            GameManager.instance.addPoints(1);
            Destroy(gameObject);
        }
    }
}
