using UnityEngine;

public abstract class TimeTrap : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        TimeSlow.OnTimeSlowed += SlowdownTrap;
        TimeSlow.OnTimeNormalized += NormalizeTrap;
    }

    protected virtual void OnDisable()
    {
        TimeSlow.OnTimeSlowed -= SlowdownTrap;
        TimeSlow.OnTimeNormalized -= NormalizeTrap;
    }

    protected abstract void SlowdownTrap(float factor);
    protected abstract void NormalizeTrap();
}
