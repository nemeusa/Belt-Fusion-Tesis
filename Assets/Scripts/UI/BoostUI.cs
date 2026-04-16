using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    private Image _boostImg;
    [SerializeField] bool _isTurnOn;

    private void Awake()
    {
        _boostImg = GetComponent<Image>();
    }
    public void ActiveBoost()
    {
        _boostImg.enabled = true;
        _isTurnOn = true;
    }

    public void DesactiveBoost()
    {
        _boostImg.enabled = false;
        _isTurnOn = false;
    }

    public bool IsTurnOn() => _isTurnOn;
}
