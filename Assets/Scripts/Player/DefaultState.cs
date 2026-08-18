using UnityEngine;

public class DefaultState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    public DefaultState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        _player.ChangeElement(TypeFSM.Default);

        //_player.meshColors.color = Color.black;
        _player.ChangeMatElement(_player.fMat, _player.fMatEye);
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

}
