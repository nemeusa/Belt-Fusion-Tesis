using UnityEngine;

public class FireState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    public FireState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        GameManager.instance.BoostContainer.ChangeSymbol(TypeFSM.Fire);
        _player.fireAura.SetActive(true);
        _player.meshColors.color = Color.red;
        _player.OnJumpPressed += JumpFire;
    }
    public void OnUpdate()
    {
        if (_player._controller.isGrounded) _player.speed = _player.initialSpeed;
    }

    public void OnExit()
    {
        _player.fireAura.SetActive(false);
        _player.OnJumpPressed -= JumpFire;
    }

    void JumpFire()
    {
        if (_player.boost <= 0)
        {
            if (_player.jumpCount > _player.maxJumps) return;
        }
        else if (_player.boost > 0 && _player.jumpCount > _player.maxJumps) _player.AddBoost(-1);
        if (_player.coyoteCounter > 0) return;

        if (_player.initialSpeed == _player.speed) _player.speed *= 1.2f;

        _player._playerVelocity.y = Mathf.Sqrt(_player._jumpFire * -3.0f * _player._gravityValue);
        //_player.StartCoroutine(_player.ActivateTrail(_player.fireTrail));
        _player.StartCoroutine(_player.ActivateParticleTrail(_player.fireParticleTrail));
        var ball = GameObject.Instantiate(_player.fireBall, _player.firePoint.transform.position, Quaternion.identity);
        GameObject.Instantiate(_player.explosionJumpPrefab, _player.firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<FireBall>().player = _player;

        _player.jumpCount += 2;
    }
}
