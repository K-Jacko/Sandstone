using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : Monster
{
    public float explosionRadius;
    public float rotationSpeed = 80.0f;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float maxHeight;
    public float shootInterval = 1f;
    public float projectileSpeed;
    public GameObject projectile;
    
    private StateMachine _stateMachine;
    private Material _spawnerMaterial;
    private LayerMask _floor;

    private bool _shooting;

    // Start is called before the first frame update
    void Awake()
    {
        traverseType = TraverseType.Air;
        Spawn();
        player = StageDirector.Instance.Player;
        monster = gameObject;
        _floor = LayerMask.GetMask("Floor");
        _spawnerMaterial = gameObject.GetComponent<MeshRenderer>().material;
        _stateMachine = new StateMachine();

        InitAttackStates();
    }

    public override void Spawn()
    {
        int layerMask = 1 << 6;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,1000,0), Vector3.down, out hit,10000, layerMask))
        {
            Debug.Log("HITTTT");
            transform.position = hit.transform.position + new Vector3(0, 10, 0);
        }
    }

    void InitAttackStates()
    {
        var idle = new Idle(this,_spawnerMaterial,player);
        var attack = new Attack(this,_spawnerMaterial,player);
        var explode = new Explode(this,_spawnerMaterial);
        
        At(idle, attack, FriendlyInRange());
        At(attack, idle, FriendlyNotInRange());
        _stateMachine.AddAnyTransition(explode, () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return distance <= explosionRadius;
        });

        _stateMachine.SetState(attack);
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> FriendlyInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance >= combatRadius);
        };

        Func<bool> FriendlyNotInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance < combatRadius);
        };
    }
    
    private void Update() => _stateMachine.Tick();

    

    void Move()
    {
        var position = player.transform.position;
        transform.RotateAround (position, Vector3.up, rotationSpeed * Time.deltaTime);
        Vector3  desiredPosition;

        //Rotate Around
        desiredPosition = ((transform.position - position).normalized * radius + position) * Mathf.Sin(1); 
        
        var distanceFromFloor = GetDistanceFromFloor();
        var warp = new Vector3(desiredPosition.x, distanceFromFloor, desiredPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, warp, Time.deltaTime * radiusSpeed);
        transform.LookAt(player.transform);
    }

    void MoveDirectly()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * radiusSpeed);
    }

    public override void Idle()
    {
        MoveDirectly();
    }
    public override void Attack()
    {
        Move();
        if(!_shooting)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        _shooting = true;
        var go = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, StageDirector.Instance.transform);
        
        go.GetComponent<Rigidbody>().velocity =
            (player.transform.position - transform.position).normalized * projectileSpeed;
         
        _shooting = false;
        yield return new WaitForSeconds(shootInterval);
    }
    float GetDistanceFromFloor()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100f, 6))
        {
            return hit.transform.position.y + maxHeight;
        }
        return maxHeight;
        
    }
    void OnDrawGizmos()
    {
        var position = gameObject.transform.position;
        Gizmos.color = new Color(0.5f,1f,0.5f,0.1f);
        Gizmos.DrawSphere(position, combatRadius);
        Gizmos.color = new Color(0.1f,0.5f,0.5f,0.1f);
        Gizmos.DrawSphere(position, explosionRadius);
    }
}

