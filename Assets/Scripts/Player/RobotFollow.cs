using System.Collections;
using UnityEngine;

public class RobotFollow : MonoBehaviour
{
    public Transform playerPos; 
    public Vector3 offset = new Vector3(-1f, 1.5f, -1f); 
    public float smoothSpeed = 0.125f;
    private Vector3 _velocity = Vector3.zero;

    public LineRenderer laserLine;

    public bool goCheckPoint;

    [SerializeField] GameObject spawnCheckpointPrefab;
    [SerializeField] Transform spawnCheckpointPoint;

    private void Awake()
    {
        //GameManager.instance.robot = this;
    }

    private void Start()
    {
        
        playerPos = GameManager.instance.player.transform;

    }

    void LateUpdate()
    {

        if (laserLine.enabled) laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, playerPos.position);

        if (!goCheckPoint)
        FollowTarget(playerPos);
    }

    public void FollowTarget(Transform target)
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);


    }


    public void CheckpointAction(Transform target)
    {
        StartCoroutine(WorkCheckpoint(target));
    }

    IEnumerator WorkCheckpoint(Transform target)
    {
        goCheckPoint = true;

        while (Vector3.Distance(target.position, transform.position) > 2.5f)
        {
            FollowTarget(target);
            Debug.Log(Vector3.Distance(target.position, transform.position));
            yield return null;

        }

        if (Vector3.Distance(target.position, transform.position) < 2.5f)
        {
            Debug.Log("el robot llega al checkpoint");
            Instantiate(spawnCheckpointPrefab, spawnCheckpointPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
            goCheckPoint = false;
        }



        //if (Vector3.Distance(target.position, transform.position) > 0.5f)
        //{
        //    FollowTarget(target);
            
        //}

        //else
        //{
        //    Instantiate(spawnCheckpointPrefab, spawnCheckpointPoint.position, Quaternion.identity);
        //    yield return new WaitForSeconds(1);
        //    goCheckPoint = false;
        //}

    }

    public void DispararRayo(TypeFSM element)
    {
        Color colorE;
        switch (element)
        {
            case TypeFSM.Fire:
                colorE = Color.red;
                break;

            case TypeFSM.Electricity:
                colorE = Color.yellow;
                break;

            case TypeFSM.Ice:
                colorE = Color.cyan;
                break;

            default:
                colorE = Color.gray;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(RoutineRayo(colorE));

    }

    IEnumerator RoutineRayo(Color color)
    {
        laserLine.enabled = true;
        laserLine.startColor = color;
        laserLine.endColor = color;

        float t = 0;
        while (t < 0.2f)
        {
            // El rayo va desde el robot al centro del player
  
            t += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("rayo");

        yield return new WaitForSeconds(0.5f);
        laserLine.enabled = false;
    }
}
