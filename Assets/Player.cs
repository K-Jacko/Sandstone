using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Friendly
{
    public enum PlayerState{Idle, Safe, Combat}

    public PlayerState State = PlayerState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        State = PlayerState.Safe;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ExitCombat()
    {
        if (State == PlayerState.Idle)
        {
            yield return new WaitForSeconds(1f);
            State = PlayerState.Safe;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MonsterSpawner>())
        {
            State = PlayerState.Combat;
            print(State);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MonsterSpawner>() && State == PlayerState.Combat)
        {
            State = PlayerState.Idle;
            print(State);
            StartCoroutine(ExitCombat());
        }
        
        
    }

    private void OnDrawGizmos()
    {
        if (State == PlayerState.Safe)
        {
            Gizmos.color = Color.green; 
        }
        else if (State == PlayerState.Combat)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(gameObject.transform.position,new Vector3(1,1,1) );
    }
}
