using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    private readonly Monster _entity;
    private Color _color = Color.green;
    private Material _entitymMaterial;
    private GameObject _player;
    

    public void Tick()
    {
        _entitymMaterial.color = _color;
        _entity.Idle();
    }

    public Idle(Monster entity, Material material, GameObject player)
    {
        _entity = entity;
        _entitymMaterial = material;
        _player = player;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}
