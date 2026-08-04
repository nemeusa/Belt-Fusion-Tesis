using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public static string _nextLevel;

    void Start()
    {
        _nextLevel = nextLevel;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerPrefs.SetInt("Nivel2Desbloqueado", 1);
            PlayerPrefs.Save();
            GameManager.instance.WinGame();
        }
    }

}
