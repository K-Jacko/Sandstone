using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : IState
{
    private readonly Player _entity;
    public void Tick()
    {
        
    }
    
    public Combat(Player entity)
    {
        _entity = entity;
    }

    public void OnEnter()
    {
        _entity.playerState = Player.PlayerState.Combat;
    }

    public void OnExit()
    {
    }
}
