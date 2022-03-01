using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginColor : IState
{
    private readonly TestEntity _entity;
    private Color _color = Color.black;
    
    public void Tick()
    {
        _entity.TEMaterial.color = _color;
    }

    public OriginColor(TestEntity entity)
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
