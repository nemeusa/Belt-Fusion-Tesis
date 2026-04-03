using UnityEngine;

public class DestructibleDevice : MonoBehaviour
{
    public TypeElement trampElement;

    private void OnTriggerEnter(Collider collision)
    {
        if (ChooseElement(collision))
        {
            GameManager.instance.addPoints(1);
            Destroy(gameObject);
        }
    }

    private bool ChooseElement(Collider other)
    {
        switch (trampElement)
        {
            case TypeElement.Fire:
                return other.gameObject.GetComponent<FireBall>();

            case TypeElement.Electricity:
                return other.gameObject.GetComponent<PlayerController>().ElectricityTrail.emitting;

            default:
                return false;
        }
    }
}

public enum TypeElement
{
    Fire,
    Electricity,
    Ice
}
