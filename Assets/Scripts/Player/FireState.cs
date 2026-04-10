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
        _player.meshColors.color = Color.red;
        _player.OnJumpPressed += JumpFire;
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        _player.OnJumpPressed -= JumpFire;
    }

    void JumpFire()
    {
        if (_player.boost <= 0)
        {
            if (_player.jumpCount > _player.maxJumps) return;
        }
        else if (_player.boost > 0 && _player.jumpCount > _player.maxJumps) _player.AddBoost(-1);
        if (_player._controller.isGrounded) return;

        _player._playerVelocity.y = Mathf.Sqrt(_player._jumpFire * -3.0f * _player._gravityValue);
        _player.StartCoroutine(_player.ActivateTrail(_player.fireTrail));
        var ball = GameObject.Instantiate(_player.fireBall, _player.firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<FireBall>().player = _player;

        _player.jumpCount += 2;
    }
}
