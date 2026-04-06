using UnityEngine;

public class DestructibleDevice : MonoBehaviour
{
    public TypeElement trampElement;
    [SerializeField] int boost = 1;

    private void OnTriggerEnter(Collider collision)
    {
        if (ChooseElement(collision))
        {
            GameManager.instance.addPoints(1);
            DetElement(collision);
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
    private void DetElement(Collider other)
    {
        if (TypeElement.Fire == trampElement)
            other.gameObject.GetComponent<FireBall>().player.AddBoost(boost);

        if (TypeElement.Electricity == trampElement)
            other.gameObject.GetComponent<PlayerController>().AddBoost(boost);

    }

}

public enum TypeElement
{
    Fire,
    Electricity,
    Ice
}
