using System.Collections;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{

    [SerializeField] Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    public void Transition()
    {
        if(ani != null)
        ani.SetBool("Trans", true);
    }

    public void ReturnTrans()
    { 
        if(ani != null)
        ani.SetBool("Trans", false);

    }
}
