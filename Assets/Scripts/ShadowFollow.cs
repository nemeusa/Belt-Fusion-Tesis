using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    public Transform playerTransform; 
    public float offsetDelSuelo = 0.05f;
    public LayerMask capasSuelo;

    void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, 50f, capasSuelo))
        {
            transform.position = hit.point + Vector3.up * offsetDelSuelo;

            float distancia = hit.distance;
            float escala = Mathf.Lerp(1.2f, 0.5f, distancia / 10f);
            transform.localScale = new Vector3(escala, escala, escala);
        }
    }
}
