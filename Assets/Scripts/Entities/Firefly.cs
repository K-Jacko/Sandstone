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

    private bool _shooting;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        InitAttackStates();
        traverseType = TraverseType.Air;
        Spawn();
    }

    void InitAttackStates()
    {
        var explode = new Explode(this,material);
        
        this._stateMachine.AddAnyTransition(explode, () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return distance <= explosionRadius;
        });
    }
    
    private void Update() => _stateMachine.Tick();


    public override void Attack()
    {
        if(!_shooting)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        _shooting = true;
        yield return new WaitForSeconds(shootInterval);
        var go = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, StageDirector.Instance.transform);
        
        go.GetComponent<Rigidbody>().velocity =
            (player.transform.position - transform.position).normalized * projectileSpeed;
         
        _shooting = false;
        
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

