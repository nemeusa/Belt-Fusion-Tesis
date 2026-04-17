using System.Collections;
using UnityEngine;

public class ElectricityState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    public ElectricityState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        GameManager.instance.BoostContainer.ChangeSymbol(TypeFSM.Electricity);
        _player.energyAura.SetActive(true);
        _player.meshColors.color = Color.yellow;
        _player.OnDashPressed += Dash;
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        _player.energyAura.SetActive(false);
        _player.OnDashPressed -= Dash;
    }

    void Dash()
    {
        if (_player.dashCount >= 1)
        {
            if (_player.boost < 1)
            return;

            else _player.AddBoost(-1);
        }

        if (!_player.isDashing)
        {
            _player.StartCoroutine(ExecuteDash());
            //_player.StartCoroutine(_player.ActivateTrail(_player.ElectricityTrail));
            _player.StartCoroutine(_player.ActivateParticleTrail(_player.electricityParticleTrail));
            _player.dashCount++;
        }
    }


    IEnumerator ExecuteDash()
    {
        _player.isDashing = true;

        float originalGravity = _player._playerVelocity.y;
        _player._playerVelocity.y = 0;

        Vector3 dashDirection = new Vector3(_player._moveInput.x, 0, _player._moveInput.y);
        if (dashDirection == Vector3.zero) dashDirection = _player.transform.forward;

        float startTime = Time.time;

        while (Time.time < startTime + _player.dashTime)
        {
            _player._controller.Move(dashDirection * _player.dashSpeed * Time.deltaTime);
            yield return null;
        }

        _player.isDashing = false;

        yield return new WaitForSeconds(_player.dashCooldown);
    }
}
