using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 _lastCheckpointPosition;
    public bool respawn;

    void Awake()
    {
        //if (Instance == null) 
        Instance = this;
        //else Destroy(gameObject);
    }

    public void UpdateCheckpoint(Vector3 newPos)
    {
        _lastCheckpointPosition = newPos;
        //Debug.Log("Checkpoint alcanzado: " + newPos);
    }

    public void Respawn(GameObject player)
    {
        StartCoroutine(ReScene(player));
        //CharacterController controller = player.GetComponent<CharacterController>();
        //if (controller != null)
        //{
        //    controller.enabled = false;
        //    player.transform.position = _lastCheckpointPosition;
        //    controller.enabled = true;
        //}
        //else
        //{
        //    player.transform.position = _lastCheckpointPosition;
        //}
    }

    IEnumerator ReScene(GameObject player)
    {
        respawn = true;
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            //controller.enabled = false;
            player.GetComponent<PlayerController>().canMove = false;
            player.transform.position = _lastCheckpointPosition;
            yield return new WaitForSeconds(0.53f);
            //controller.enabled = true;
            player.GetComponent<PlayerController>().canMove = true;
            respawn = false;
        }
    }
}
