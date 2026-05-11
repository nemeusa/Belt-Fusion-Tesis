using System.Collections;
using UnityEngine;

public class RobotFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(-1f, 1.5f, -1f); 
    public float smoothSpeed = 0.125f;
    private Vector3 _velocity = Vector3.zero;

    public LineRenderer laserLine;


    private void Awake()
    {
        target = GameManager.instance.player.transform;
        //GameManager.instance.robot = this;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothSpeed);

        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, target.position);
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
