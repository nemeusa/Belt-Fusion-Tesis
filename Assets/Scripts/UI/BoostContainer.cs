using TMPro;
using UnityEngine;

public class BoostContainer : MonoBehaviour
{
    [SerializeField] BoostUI[] _boosts;

    [SerializeField] GameObject _defaultSimbol;
    [SerializeField] GameObject _fireSimbol;
    [SerializeField] GameObject _energySimbol;
    [SerializeField] GameObject _iceSimbol;

    [SerializeField] GameObject _offDefaultSimbol;
    [SerializeField] GameObject _offFireSimbol;
    [SerializeField] GameObject _offEnergySimbol;
    [SerializeField] GameObject _offIceSimbol;

    public float seconds;
    [SerializeField] TMP_Text counterText;

    string counter;

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
        if (GameManager.instance.winGame)
        {
            GameManager.instance.counterGame = counter;
            return;

        }

        seconds += Time.deltaTime;

        int minutos = Mathf.FloorToInt(seconds / 60);
        int segs = Mathf.FloorToInt(seconds % 60);

        // Multiplicamos el resto decimal por 100 para obtener dos dígitos de milisegundos
        int milisegundos = Mathf.FloorToInt((seconds % 1) * 100);

        counter = string.Format("{0:00}:{1:00}:{2:00}", minutos, segs, milisegundos);

        // Agregamos el tercer campo {2:00} al formato del string
        counterText.text = counter;

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
        DesactivateSymbol(newElement).SetActive(false);

        if (newElement == oldElement) return;
        ActivateSymbol(oldElement).SetActive(false);
        DesactivateSymbol(oldElement).SetActive(true);

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

    GameObject DesactivateSymbol(TypeFSM newElement)
    {
        switch (newElement)
        {
            case TypeFSM.Fire:
                return _offFireSimbol;

            case TypeFSM.Electricity:
                return _offEnergySimbol;

            case TypeFSM.Ice:
                return _offIceSimbol;

            default:
                return _offDefaultSimbol;
        }

    }
}
