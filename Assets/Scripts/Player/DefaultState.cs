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
        _player.canDash = false;
        _player.maxJumps = 1;
        _player.GetComponent<MeshRenderer>().material.color = Color.black;
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

}
