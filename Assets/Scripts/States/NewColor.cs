using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewColor : IState
{
    private readonly TestEntity _entity;
    private Color _color = Color.white;
    private IEnumerator _enumerator;
    
    public void Tick()
    {
        _entity.TEMaterial.color = _color;
    }

    public NewColor(TestEntity entity)
    {
        _entity = entity;
    }

    public void OnEnter()
    {
        _enumerator = _entity.Leaving();
        if(!_entity.leaving)
            _entity.StartCoroutine(_enumerator);
    }

    public void OnExit()
    {
        
    }
}
