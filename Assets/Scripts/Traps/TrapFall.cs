using System.Collections;
using UnityEngine;

public class TrapFall : MonoBehaviour
{
    public float fallTime = 1.5f;   
    public float speedFall = 5f; 
    public float respawnTime = 3f; 

    Vector3 initialPos;
    public bool falling = false;


    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        if (falling)
        {
            transform.Translate(Vector3.down * speedFall * Time.deltaTime);
        }
    }

    public IEnumerator DownFall()
    {
        float timer = 0;
        Vector3 tempPos = transform.position;

        while (timer < fallTime)
        {
            transform.position = tempPos + Random.insideUnitSphere * 0.05f;
            timer += Time.deltaTime;
            yield return null;
        }

        falling = true;

        yield return new WaitForSeconds(respawnTime);
        ResetPlatform();
    }

    void ResetPlatform()
    {
        falling = false;
        transform.position = initialPos;
    }
}
