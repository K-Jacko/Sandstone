using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : IState
{
    private readonly Monster _entity;
    private Color _color = Color.white;
    private IEnumerator _enumerator;
    private Material _entityMaterial;
    private GameObject _player;
    
    public void Tick()
    {
        _entityMaterial.color = _color;
        
    }

    public Cooldown(Monster entity, Material material, GameObject player)
    {
        _entity = entity;
        _entityMaterial = material;
        _player = player;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}
