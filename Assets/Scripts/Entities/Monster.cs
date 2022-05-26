using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Entity
{
    
    public float combatRadius;
    public GameObject player;
    
    public GemElement GemElement;
    public Material material;

    public StateMachine _stateMachine;


    public enum TraverseType
    {
        None,
        Ground,
        Air
    };

    public TraverseType traverseType;


    public virtual void Init()
    {
        //material = gameObject.GetComponentInChildren<MeshRenderer>().material;
        player = StageDirector.Instance.Player.gameObject;
        _stateMachine = new StateMachine();
        InitAttackStates();
    }
    public virtual void Spawn()
    {
        int layerMask = 1 << 6;
        RaycastHit hit;
        if (traverseType == TraverseType.Air)
        {
            if (Physics.Raycast(transform.position + new Vector3(0,1000,0), Vector3.down, out hit,10000, layerMask))
            {
                transform.position = hit.point + new Vector3(0, 10, 0);
            }
        }
        else if(traverseType == TraverseType.Ground)
        {
            if (Physics.Raycast(transform.position + new Vector3(0,1000,0), Vector3.down, out hit,10000, layerMask))
            {
                transform.position = hit.point;
            }
        }
        
    }
    public virtual void InitAttackStates()
    {
        var idle = new Idle(this,material,player.gameObject);
        var attack = new Attack(this,material,player.gameObject);
        
        At(attack, idle, PlayerInRange());
        At(idle, attack, PlayerNotInRange());
        
        _stateMachine.SetState(idle);
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        
        Func<bool> PlayerNotInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, player.gameObject.transform.position);
            return !(distance >= combatRadius);
        };
        
        Func<bool> PlayerInRange() => () =>
        {
            var distance = Vector3.Distance(transform.position, player.transform.position);
            return !(distance < combatRadius);
        };
    }
    
    public virtual void Attack()
    {
        
    }
    
    public virtual void Idle()
    {
        //MoveDirectly
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * Agility);
    }
    
    public void Kill()
    {
        Destroy(gameObject);
    }
    
    void OnDrawGizmos()
    {
        var position = gameObject.transform.position;
        Gizmos.color = new Color(1.0f,0f,0.0f,0.1f);
        Gizmos.DrawSphere(position, combatRadius);
    }
}
