using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ElectricityState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    Vector3 _direccionDash;

    public ElectricityState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        _player.ChangeElement(TypeFSM.Electricity);
        //_player.energyAura.SetActive(true);
        //_player.meshColors.color = Color.yellow;
        _player.ChangeMatElement(_player.electricityMat[0], _player.electricityMatEye[0]);
        _player.ChangeSkinElement(_player.electricityMat, _player.electricityMatEye, _player.electricityMeshes);

        _player.OnDashPressed += Dash;
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        //_player.energyAura.SetActive(false);
        _player.OnDashPressed -= Dash;
        _player.meshColors.color = Color.yellow;
        _player.isDashing = false;
        DashEffects();

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
            _player.StartCoroutine(_player.CamEffects());
            _player.dashCount++;
        }
    }


    IEnumerator ExecuteDash()
    {
        _player.isDashing = true;
        _player.CountMoves(1);

        DashEffects();
        float originalGravity = _player._playerVelocity.y;
        _player._playerVelocity.y = 0;

        Vector3 dashDirection = new Vector3(_player.moveInput.x, 0, _player.moveInput.y);
        if (dashDirection == Vector3.zero) dashDirection = _player.transform.forward;

        float startTime = Time.time;

        while (Time.time < startTime + _player.dashTime && !_player.dontMovePlayer && _player.isDashing)
        {
            _player._controller.Move(dashDirection * _player.dashSpeed * Time.deltaTime);
            yield return null;
        }

        _player.isDashing = false;
        DashEffects();

        yield return new WaitForSeconds(_player.dashCooldown);
    }

    void DashEffects()
    {

        if (_player.isDashing)
        {
            if (_player.invisibleInDash)
            {
                Color col = _player.meshColors.color;
                col.a = 0;
                _player.meshColors.color = col;
            }

            _player.audioSource.PlayOneShot(_player.dashAudio);


            _player.VibrarControl(1, 0.4f, 0.5f);

            foreach (var d in _player.fbxDashList) d.SendEvent("OnPlay");
            IniciarDash();
        }

        else
        {
            if (_player.invisibleInDash)
            _player.meshColors.color = Color.yellow;

            foreach (var d in _player.fbxDashList) d.SendEvent("OnStop");
        }

    }

    void IniciarDash()
    {
        Vector3 inputDireccion = new Vector3(_player.moveInput.x, 0, _player.moveInput.y);

        if (inputDireccion.sqrMagnitude > 0.1f)
        {
            _direccionDash = inputDireccion.normalized;
        }
        else
        {
            _direccionDash = _player.transform.forward;
        }

        foreach (var p in _player.dashParticules)
        {

            var d = GameObject.Instantiate(p, _player.transform.position, Quaternion.identity);
            d.transform.forward = _direccionDash;
            GameObject.Destroy(d, 2);
        }
    }
}
