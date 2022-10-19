using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Entity
{
    
    [Header("Stats/Card")]
    public MobCard mobCard;
    [Header("Cosmetic")]
    public Material material;

    [Header("Combat")] public MovementType movementType;
    public float combatRadius;
    public GameObject target;
    public NavMeshAgent agent;
    
    
    public StateMachine movementStateMachine;
    public StateMachine combatStateMachine;

    public virtual void Init(SpawnData data)
    {
        ProcessStats();
        target = data.target;
        InitNaveMesh();
        InitAttackStates();
    }

    void ProcessStats()
    {
        mobCard.stats = new Stats(1, 1, 3, 10);
        stats = mobCard.stats;
        InitManaPool();
    }
    void InitManaPool()
    {
        if (SceneDirector.Instance)
        {
            ManaPool = stats.Focus * SceneDirector.Instance.coEef;
        }
        else
        {
            ManaPool = stats.Focus;
        }
    }
    public void Spawn()
    {
        
    }

    public virtual void InitNaveMesh()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    public virtual void InitAttackStates()
    {

        movementStateMachine = new StateMachine();
        combatStateMachine = new StateMachine();
        
        var persue = new Persue(this,target);
        var avoid = new Avoid(this,target);
        var idle = new Idle();
        var attack = new Attack(this, target);
        
        movementStateMachine.AddAnyTransition(persue,PlayerNotInRange());
        movementStateMachine.AddAnyTransition(avoid,PlayerInRange());
        combatStateMachine.AddAnyTransition(idle,PlayerNotInRange());
        combatStateMachine.AddAnyTransition(attack,PlayerInRange());
        
        movementStateMachine.SetState(persue);
        combatStateMachine.SetState(idle);
        
        Func<bool> PlayerNotInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, target.gameObject.transform.position);
            return (distance >= combatRadius);
        };
        //
        Func<bool> PlayerInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            return (distance < combatRadius);
        };
    }

    void Update()
    {
        movementStateMachine.Tick();
        combatStateMachine.Tick();
    }
    
    public void Kill()
    {
        
    }
    
    public enum MovementType
    {
        Flying,
        Walking
    };
    
    void OnDrawGizmos()
    {
        var position = gameObject.transform.position;
        Gizmos.color = new Color(1.0f,0f,0.0f,0.1f);
        Gizmos.DrawSphere(position, combatRadius);
    }
}
