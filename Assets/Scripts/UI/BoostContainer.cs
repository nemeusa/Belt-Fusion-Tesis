using TMPro;
using UnityEngine;

public class BoostContainer : MonoBehaviour
{
    [SerializeField] BoostUI[] _boosts;

    [SerializeField] GameObject _defaultSimbol;
    [SerializeField] GameObject _fireSimbol;
    [SerializeField] GameObject _energySimbol;
    [SerializeField] GameObject _iceSimbol;
    float seconds;
    [SerializeField] TMP_Text counterText;

    TypeFSM oldElement = TypeFSM.Default;

    private void Awake()
    {
        oldElement = TypeFSM.Default;
    }

    private void Update()
    {
        TimerCount();
    }

    void TimerCount()
    {
        seconds += Time.deltaTime;

        int minutos = Mathf.FloorToInt(seconds / 60);
        int segs = Mathf.FloorToInt(seconds % 60);

        // Multiplicamos el resto decimal por 100 para obtener dos dígitos de milisegundos
        int milisegundos = Mathf.FloorToInt((seconds % 1) * 100);

        // Agregamos el tercer campo {2:00} al formato del string
        counterText.text = string.Format("{0:00}:{1:00}:{2:00}", minutos, segs, milisegundos);

        //seconds += Time.deltaTime;

        //int minutos = Mathf.FloorToInt(seconds / 60);
        //int segs = Mathf.FloorToInt(seconds % 60);

        //counterText.text = string.Format("{0:00}:{1:00}", minutos, segs);
    }
    public void BoostsActive(int actualBoost)
    {
        for (int i = 0; i < _boosts.Length; i++)
        {
            if (i < actualBoost) _boosts[i].ActiveBoost();

            else _boosts[i].DesactiveBoost();
        }
    }

    public void ChangeSymbol(TypeFSM newElement)
    {

        ActivateSymbol(newElement).SetActive(true);

        if (newElement == oldElement) return;
        ActivateSymbol(oldElement).SetActive(false);

        oldElement = newElement;
    }

    GameObject ActivateSymbol(TypeFSM newElement)
    {
        switch (newElement)
        {
            case TypeFSM.Fire :
            return _fireSimbol;

            case TypeFSM.Electricity:
                return _energySimbol;

            case TypeFSM.Ice:
                return _iceSimbol;

            default:
                return _defaultSimbol;
        }

    }
}
