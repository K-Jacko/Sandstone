using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : IState
{
    private readonly Player _entity;
    public void Tick()
    {
        
    }

    public Safe(Player entity)
    {
        _entity = entity;
    }

    public void OnEnter()
    {
        _entity.playerState = Player.PlayerState.Safe;
    }

    public void OnExit()
    {
        
    }
}
