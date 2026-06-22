using Unity.VisualScripting;
using UnityEngine;

public class DestructibleDevice : MonoBehaviour
{
    [Header ("General")]
    public TypeFSM trampElement;
    [SerializeField] bool useAnyElements;
    [SerializeField] int boost = 1;
    [SerializeField] GameObject desObj;

    [Header ("Extra effects")]
    [SerializeField] ParticleSystem spawnParticles;
    [SerializeField] AudioClip destroySound;
    [SerializeField] Animator aniController;

    bool act;

    private void Start()
    {
        if (useAnyElements) trampElement = TypeFSM.Default;

    }

    private void Update()
    {
        if (CheckpointManager.Instance.respawn)
        {
            desObj.SetActive(true);
            //act = true;
            gameObject.GetComponent<Collider>().enabled = true;
            if (aniController != null) aniController.SetBool("Destroy", false);
        }

    }

    private void OnTriggerStay(Collider collision)
    {
        if ((ChooseElement(collision) || SearchAllElements(collision)) && desObj && !act)
        {

            desObj.SetActive(false);
            DetElement(collision);
            act = false;
            gameObject.GetComponent<Collider>().enabled = false;
            if (spawnParticles != null) spawnParticles.Play();
            if (destroySound != null) GameManager.instance.PlaySound(destroySound);
            if (aniController != null)
            {
                aniController.SetBool("Destroy", true);
                Debug.Log("hizo la animacion");
            }
        }
    }

    

    private bool ChooseElement(Collider other)
    {
        if (useAnyElements) return false;

        switch (trampElement)
        {
            case TypeFSM.Fire:
                return other.gameObject.GetComponent<FireBall>();

            case TypeFSM.Electricity:
                if (other.gameObject.GetComponent<PlayerController>() != null)
                    return other.gameObject.GetComponent<PlayerController>().isDashing;
                else return false;

            default:
                return false;
        }
    }

    private bool SearchAllElements(Collider other)
    {
        if (!useAnyElements) return false;

        if (other.GetComponent<FireBall>() != null) return true;

        else if (other.gameObject.GetComponent<PlayerController>() != null) return other.gameObject.GetComponent<PlayerController>().isDashing;

        else return false;
    }

    private void DetElement(Collider other)
    {
        switch (trampElement)
        {
            case TypeFSM.Fire:
                other.gameObject.GetComponent<FireBall>().player.AddBoost(boost);
                break;  

            case TypeFSM.Electricity:
                other.gameObject.GetComponent<PlayerController>().AddBoost(boost);
                break;


            default:
                GameManager.instance.player.AddBoost(boost);
                break;

        }
    }

}