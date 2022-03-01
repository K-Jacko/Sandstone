using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleColor : IState
{
    private readonly TestEntity _entity;
    private Color _color = Color.green;
    

    public void Tick()
    {
        _entity.TEMaterial.color = _color;
    }

    public IdleColor(TestEntity entity)
    {
        _entity = entity;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}
