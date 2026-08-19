using UnityEngine;

public class IceState : State
{
    FSM<TypeFSM> _fsm;
    PlayerController _player;

    public IceState(FSM<TypeFSM> fsm, PlayerController player)
    {
        _fsm = fsm;
        _player = player;
    }

    public void OnEnter()
    {
        _player.ChangeElement(TypeFSM.Ice);

        //_player.meshColors.color = Color.blue;
        _player.ChangeMatElement(_player.iceMat[0], _player.iceMatEye[0]);
        _player.ChangeSkinElement(_player.iceMat, _player.iceMatEye, _player.iceMeshes);
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
    }
}
