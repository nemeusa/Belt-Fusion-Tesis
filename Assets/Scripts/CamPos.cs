using Unity.Cinemachine;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    [SerializeField] PlayerController player;
    CinemachineFollow camFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camFollow = GetComponent<CinemachineFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player._controller.isGrounded)
        {
            camFollow.FollowOffset.y = 3.55f;
        }
        else camFollow.FollowOffset.y = 7.2f;

    }
}
