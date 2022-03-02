using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : IState
{
    private readonly Monster _entity;
    private Material _entityMaterial;
    private Color _color = Color.red;

    public void Tick()
    {
        _entityMaterial.color = _color;
        _entity.transform.localScale += new Vector3(0.0025f,0.0025f,0.0025f);
        if (_entity.transform.localScale.x > 1f)
            _entity.Kill();
    }
    
    public Explode(Monster entity, Material material)
    {
        _entity = entity;
        _entityMaterial = material;
    }

    public void OnEnter()
    {
        
    }
    
    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
