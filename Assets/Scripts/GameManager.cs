using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public PlayerController player;
    public BoostContainer BoostContainer;
    //public TMP_Text pointsText;
    //public TMP_Text boostText;
    //int pointsCount;

    private void Awake()
    {
       instance = this;
    }

    //private void Update()
    //{
    //    //if(player == null) Debug.LogError("pone el player en el GameManager crack");
    //}
    //public void addPoints(int add)
    //{
    //    pointsCount += add;
    //   //pointsText.text = $"Interuptores: {pointsCount}";
    //}
}
