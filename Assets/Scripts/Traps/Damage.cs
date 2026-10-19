using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;


public class Damage : MonoBehaviour
{

    [SerializeField] AudioClip deathAudio;
    [SerializeField] ParticleSystem deathEffects;
    [SerializeField] float deathDuration = 0.2f;

    [SerializeField] bool fallDeath;

    Transform playerMesh;

    bool isFlyDeath;

   
    [SerializeField] Direction direction;

    Vector3 vectorDir;

    [SerializeField] float speedOutCamera = 10;

    


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (fallDeath)
            {
                playerMesh = player.meshFather.transform;

                StartCoroutine(OutCamDeath());
            }


                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, deathEffects);

        }
    }

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += vectorDir * speedOutCamera * Time.deltaTime;


    }


    IEnumerator OutCamDeath()
    {
        isFlyDeath = true;

        if (direction == Direction.Up) vectorDir = Vector3.up;

        switch (direction)
        {
            case Direction.Up:
                vectorDir = Vector3.up;
                break;

            case Direction.Down:
                vectorDir = Vector3.down;
                break;

            case Direction.Left:
                vectorDir = Vector3.left;
                break;

            case Direction.Right:
                vectorDir = Vector3.right;
                break;

            case Direction.Back:
                vectorDir = Vector3.back;
                break;

            default:
                vectorDir = Vector3.forward;
                break;
        }



        yield return new WaitForSeconds(deathDuration);

        isFlyDeath = false;
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Back,
        Forward
    }
}
