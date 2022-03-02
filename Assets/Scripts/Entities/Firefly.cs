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
    
    private StateMachine _stateMachine;
    private Material _spawnerMaterial;
    // Start is called before the first frame update
    void Awake()
    {
        player = StageDirector.GetPlayer();
        monster = gameObject;
        spawnPoint = transform.position;
        _spawnerMaterial = GetComponent<MeshRenderer>().material;
        _stateMachine = new StateMachine();
        
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

        _stateMachine.SetState(idle);
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> FriendlyInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance > spawnRadius);
        };

        Func<bool> FriendlyNotInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance <= combatRadius);
        };
    }
    
    private void Update() => _stateMachine.Tick();

    public override void Attack()
    {
        var oldRadius = radius;
        var position = player.transform.position;
        transform.RotateAround (position, Vector3.up, rotationSpeed * Time.deltaTime);
        radius *= Time.deltaTime - 0.05f; 
        var desiredPosition = (transform.position - position).normalized * radius + position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,1f,1f,0.2f);
        var position = gameObject.transform.position;
        Gizmos.DrawSphere(position, spawnRadius);
        Gizmos.color = new Color(0.5f,1f,0.5f,0.1f);
        Gizmos.DrawSphere(position, combatRadius);
        Gizmos.color = new Color(0.1f,0.5f,0.5f,0.1f);
        Gizmos.DrawSphere(position, explosionRadius);
    }
}

