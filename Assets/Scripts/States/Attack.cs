using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IState
{
    private readonly Monster _entity;
    private Color _color = Color.black;
    private Material entityMaterial;
    private GameObject _player;
    
    public void Tick()
    {
        entityMaterial.color = _color;
        _entity.Attack();
    }

    public Attack(Monster entity, Material material, GameObject player)
    {
        _entity = entity;
        entityMaterial = material;
        _player = player;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
       
    }
}
