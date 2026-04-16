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
        GameManager.instance.BoostContainer.ChangeSymbol(TypeFSM.Ice);
        _player.meshColors.color = Color.blue;
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
    }
}
