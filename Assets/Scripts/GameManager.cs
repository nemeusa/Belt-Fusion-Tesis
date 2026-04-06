using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text pointsText;
    public TMP_Text boostText;
    int pointsCount;

    private void Awake()
    {
        instance = this;
    }
    public void addPoints(int add)
    {
        pointsCount += add;
        pointsText.text = $"Interuptores: {pointsCount}";
    }
}
