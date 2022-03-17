using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float health;
    public float combatRadius;
    public GameObject player;
    public GameObject monster;
    public float speed;
    public GemElement GemElement;

    public enum TraverseType
    {
        None,
        Ground,
        Air
    };

    public TraverseType traverseType;
    
    public virtual void Spawn()
    {
        
    }
    
    public virtual void Attack()
    {
        
    }
    
    public void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Idle()
    {
        
    }
}
