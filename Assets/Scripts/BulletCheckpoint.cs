using UnityEngine;

public class BulletCheckpoint : MonoBehaviour
{
    [SerializeField] private float velocidad = 20f;

    private Vector3 direccion;
    private bool tieneObjetivo = false;

    Transform objetivo;

    CheckpointTrigger checkpointCode;

    void Update()
    {
        if (tieneObjetivo)
        {
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

            if (Vector3.Distance(objetivo.position, transform.position) < 2)
            {
                checkpointCode.CreateCheckpoint();
                Destroy(gameObject);
            }
        }
    }

    public void ConfigurarDestino(Transform posicionDestino, CheckpointTrigger checkCode)
    {
        objetivo = posicionDestino;
        direccion = (posicionDestino.position - transform.position).normalized;

        checkpointCode = checkCode;

        tieneObjetivo = true;
    }
}
