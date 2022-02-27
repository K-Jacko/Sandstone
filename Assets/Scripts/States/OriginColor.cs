using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginColor : IState
{
    private readonly TestEntity _entity;

    public OriginColor(TestEntity entity)
    {
        _entity = entity;
    }
    public void Tick()
    {
        _entity.Target = ChooseRandomColor();
    }

    private ChooseRandomColor()
    {
        
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
