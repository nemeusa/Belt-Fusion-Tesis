using UnityEngine;

public class DestructibleDevice : MonoBehaviour
{
    public TypeFSM trampElement;
    [SerializeField] int boost = 1;

    private void OnTriggerEnter(Collider collision)
    {
        if (ChooseElement(collision))
        {
            DetElement(collision);
            Destroy(gameObject);
        }
    }

    private bool ChooseElement(Collider other)
    {
        switch (trampElement)
        {
            case TypeFSM.Fire:
                return other.gameObject.GetComponent<FireBall>();

            case TypeFSM.Electricity:
                return other.gameObject.GetComponent<PlayerController>().ElectricityTrail.emitting;

            default:
                return false;
        }
    }
    private void DetElement(Collider other)
    {
        if (TypeFSM.Fire == trampElement)
            other.gameObject.GetComponent<FireBall>().player.AddBoost(boost);

        if (TypeFSM.Electricity == trampElement)
            other.gameObject.GetComponent<PlayerController>().AddBoost(boost);

    }

}