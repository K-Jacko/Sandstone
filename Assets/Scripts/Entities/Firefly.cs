using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : Monster
{
    public float explosionRadius;
    public float maxHeight;
    public float shootInterval = 1f;
    public float projectileSpeed;
    public GameObject projectile;
    
    private LayerMask _floor;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public override void Init()
    {
        base.Init();
        InitAttackStates();
        ManaPool = Focus;
        traverseType = TraverseType.Air;
        Spawn();
    }

    public override void InitAttackStates()
    {
        // base.InitAttackStates();
        // var explode = new Explode(this,material);
        //
        // this._stateMachine.AddAnyTransition(explode, () =>
        // {
        //     var distance = Vector3.Distance(transform.position, player.transform.position);
        //     return distance <= explosionRadius;
        // });
    }
    
    private void Update() => _stateMachine.Tick();


    public override void Attack()
    {
        if (!isCasting && ManaPool >= ability.manaCost)
        {
            isCasting = true;
            Cast(ability, player.GetComponent<Entity>());
            
        }
        else
        {
            Refactor();
        }
    }

    float GetDistanceFromFloor()
    {
        int layerMask = 1 << 6;
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100f, layerMask))
        {
            return hit.point.y + maxHeight;
        }
        return maxHeight;
        
    }
    
}

