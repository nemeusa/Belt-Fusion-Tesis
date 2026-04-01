using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

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
        _player.maxJumps = 2;
        _player.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        _player.maxJumps = 1;
    }
}
