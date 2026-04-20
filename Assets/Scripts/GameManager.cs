using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]public PlayerController player;
    public BoostContainer BoostContainer;

    private void Awake()
    {
       instance = this;
    }
}
