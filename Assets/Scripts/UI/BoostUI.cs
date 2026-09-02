using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    private Image _boostImg;
    [SerializeField] bool _isTurnOn;
    private Animator _animator;

    private void Awake()
    {
        _boostImg = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }
    public void ActiveBoost()
    {
        _boostImg.enabled = true;
        _animator.SetTrigger("IsActive");
        _isTurnOn = true;
    }

    public void DesactiveBoost()
    {
        _boostImg.enabled = false;
        _isTurnOn = false;
    }

    public bool IsTurnOn() => _isTurnOn;
}
