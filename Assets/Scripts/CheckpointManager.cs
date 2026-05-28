using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 _lastCheckpointPosition;
    public bool respawn;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateCheckpoint(Vector3 newPos)
    {
        _lastCheckpointPosition = newPos;
        //Debug.Log("Checkpoint alcanzado: " + newPos);
    }

    public void Respawn(GameObject player)
    {
        StartCoroutine(ReScene(player));

    }

    IEnumerator ReScene(GameObject player)
    {
        respawn = true;
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            //controller.enabled = false;
            //player.GetComponent<PlayerController>().isDeath = true;
            player.transform.position = _lastCheckpointPosition;
            yield return new WaitForSeconds(0.53f);
            //player.GetComponent<PlayerController>().isDeath = false;
            //controller.enabled = true;
            respawn = false;
        }
    }
}
