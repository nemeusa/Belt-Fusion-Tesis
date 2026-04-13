using UnityEngine;

public class MakeChildren : MonoBehaviour
{
    [SerializeField] GameObject father;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Es hijo");
            other.transform.SetParent(father.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            other.transform.SetParent(null);
            Debug.Log("Dejo de ser hijo");
        }
    }
}
