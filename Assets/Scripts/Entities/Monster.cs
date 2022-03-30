using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public float health;
    public float combatRadius;
    public GameObject player;
    public GameObject monster;
    public float speed;
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
        material = gameObject.GetComponent<MeshRenderer>().material;
        player = StageDirector.Instance.Player.gameObject;
        _stateMachine = new StateMachine();
        monster = gameObject;
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
                transform.position = hit.point + new Vector3(0,GetComponent<Collider>().bounds.extents.y + 1,0);
            }
        }
        
    }
    void InitAttackStates()
    {
        var idle = new Idle(this,material,player);
        var attack = new Attack(this,material,player);

        At(attack, idle, FriendlyInRange());
        At(idle, attack, FriendlyNotInRange());

        _stateMachine.SetState(idle);
        
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> FriendlyNotInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance >= combatRadius);
        };

        Func<bool> FriendlyInRange() => () =>
        {
            var distance = Vector3.Distance(monster.transform.position, player.transform.position);
            return !(distance < combatRadius);
        };
    }
    
    public virtual void Attack()
    {
        
    }
    
    public virtual void Idle()
    {
        //MoveDirectly
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
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
