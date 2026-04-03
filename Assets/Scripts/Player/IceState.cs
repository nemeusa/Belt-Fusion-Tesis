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
        _player.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
    }
}
