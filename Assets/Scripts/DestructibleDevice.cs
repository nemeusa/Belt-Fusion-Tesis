using UnityEngine;

public class DestructibleDevice : MonoBehaviour
{
    public TypeFSM trampElement;
    [SerializeField] int boost = 1;

    [SerializeField] GameObject desObj;
    bool act;

    private void OnTriggerStay(Collider collision)
    {
        if (ChooseElement(collision) && desObj && !act)
        {
            DetElement(collision);
            Debug.Log("energia xd");
            desObj.SetActive(false);
            act = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private bool ChooseElement(Collider other)
    {
        switch (trampElement)
        {
            case TypeFSM.Fire:
                return other.gameObject.GetComponent<FireBall>();

            case TypeFSM.Electricity:
                return other.gameObject.GetComponent<PlayerController>().isDashing;

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