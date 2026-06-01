using System.Collections;
using UnityEngine;

public class RobotFollow : MonoBehaviour
{
    public Transform playerPos;
    public Vector3 offset = new Vector3(-1f, 1.5f, -1f);
    public float smoothSpeed = 0.125f;
    private Vector3 _velocity = Vector3.zero;

    public MeshRenderer[] meshRobot;

    //public LineRenderer laserLine;

    public bool goCheckPoint;

    [SerializeField] GameObject bulletCheckpointPrefab;
    [SerializeField] Transform spawnCheckpointPoint;

    Transform _checkpointPoint;
    CheckpointTrigger _checkpointCode;


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

        //if (laserLine.enabled) laserLine.SetPosition(0, transform.position);
        //laserLine.SetPosition(1, playerPos.position);

        if (!goCheckPoint)
            FollowTarget(playerPos);

        else GoCheckpoint();
    }

    public void FollowTarget(Transform target)
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);


    }


    public void CheckpointAction(Transform target, CheckpointTrigger checkpointCode)
    {
        goCheckPoint = true;


        Debug.Log("el robot dispara el checkpoint");



        _checkpointPoint = target;
        _checkpointCode = checkpointCode;


        //var c = Instantiate(bulletCheckpointPrefab, spawnCheckpointPoint.position, Quaternion.identity);

        //c.GetComponentInChildren<BulletCheckpoint>().ConfigurarDestino(target, checkpointCode);




    }

    public void GoCheckpoint()
    {
        if (Vector3.Distance(_checkpointPoint.position, transform.position) < 1)
        { 
            transform.forward = playerPos.position;

            transform.position = _checkpointPoint.position;

            if(_checkpointCode.activated) return;

            _checkpointCode.CreateCheckpoint();

            //_checkpointPoint = null;
            //_checkpointCode = null;
        }
        // FollowTarget(_checkpointPoint);

        Vector3 dir = (_checkpointPoint.position - transform.position).normalized;

        transform.Translate(dir * 40 * Time.deltaTime, Space.World);


        Debug.Log("Busca checkpoint");

    }

    //IEnumerator WorkCheckpoint(Transform target, GameObject bullet, CheckpointTrigger checkpointCode)
    IEnumerator WorkCheckpoint(float dist, GameObject bullet, CheckpointTrigger checkpointCode)
    {
        while (bullet != null)
        {
            Debug.Log(dist);

           
            if (dist < 5f)
            {
                Debug.Log("llego y se creo el checkpoint");

                checkpointCode.CreateCheckpoint();
                Destroy(bullet);
            }
            yield return null;
        }
   
    }

    //IEnumerator WorkCheckpoint(Transform target)
    //{
    //    goCheckPoint = true;

    //    if (Vector3.Distance(target.position, transform.position) < 2.5f)
    //    {
    //        Debug.Log("el robot dispara el checkpoint");
    //        var c = Instantiate(bulletCheckpointPrefab, spawnCheckpointPoint.position, Quaternion.identity);

    //        if (Vector3.Distance(target.position, c.transform.position) < 2.5f)

    //        goCheckPoint = false;
    //    }



    //    //if (Vector3.Distance(target.position, transform.position) > 0.5f)
    //    //{
    //    //    FollowTarget(target);

    //    //}

    //    //else
    //    //{
    //    //    Instantiate(spawnCheckpointPrefab, spawnCheckpointPoint.position, Quaternion.identity);
    //    //    yield return new WaitForSeconds(1);
    //    //    goCheckPoint = false;
    //    //}

    //}

    //public void DispararRayo(TypeFSM element)
    //{
    //    Color colorE;
    //    switch (element)
    //    {
    //        case TypeFSM.Fire:
    //            colorE = Color.red;
    //            break;

    //        case TypeFSM.Electricity:
    //            colorE = Color.yellow;
    //            break;

    //        case TypeFSM.Ice:
    //            colorE = Color.cyan;
    //            break;

    //        default:
    //            colorE = Color.gray;
    //            break;
    //    }

    //    StopAllCoroutines();
    //    StartCoroutine(RoutineRayo(colorE));

    //}

    //IEnumerator RoutineRayo(Color color)
    //{
    //    laserLine.enabled = true;
    //    laserLine.startColor = color;
    //    laserLine.endColor = color;

    //    float t = 0;
    //    while (t < 0.2f)
    //    {
    //        // El rayo va desde el robot al centro del player

    //        t += Time.deltaTime;
    //        yield return null;
    //    }
    //    //Debug.Log("rayo");

    //    yield return new WaitForSeconds(0.5f);
    //    laserLine.enabled = false;
    //}
}
