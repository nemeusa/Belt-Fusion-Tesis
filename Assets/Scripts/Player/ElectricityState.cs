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
        _player.GetComponent<MeshRenderer>().material.color = Color.yellow;
        _player.canDash = true;
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        _player.canDash = false;
    }

}
