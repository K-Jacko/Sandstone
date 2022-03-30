using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState{Safe,Combat,Home}

    public PlayerState playerState = PlayerState.Home;
    public int health = 5;
    public float distanceTickThreshold = 25f;
    public static Action OnPlayerDistanceTick;
    public static Vector2 PlayerPosition;
    
    private Vector2 _oldPosition;
    private bool inSafeZone;
    private StateMachine _stateMachine;
    
    // Start is called before the first frame update
    void Start()
    {
        Projectile.onHitPlayer += Hit;
        _stateMachine = new StateMachine();
        InitAttackStates();
        
    }

    // Update is called once per frame
    void Update()
    {
        //1 = scale of map. Find way to change reff to stageDirector
        PlayerPosition = new Vector2(transform.position.x, transform.position.z) / 1;
        if((_oldPosition - PlayerPosition).sqrMagnitude > distanceTickThreshold)
        {
            _oldPosition = PlayerPosition;
            OnPlayerDistanceTick?.Invoke();
        }
        _stateMachine.Tick();
    }

    void Hit()
    {
        health -= 1;
    }
    
    void InitAttackStates()
    {
        //When a new safeezone is generated use an action to tell stageDirtector to reorganise the arra
        var safeZone = StageDirector.Instance.safeZones[0].GetComponentInChildren<MeshRenderer>();
        var safe = new Safe(this);
        var combat = new Combat(this);
        var home = new Home();
        
        At(combat, safe, InsideSafeZone());
        At(safe, combat,OutsideSafeZone());

        
        _stateMachine.SetState(safe);
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        
        Func<bool> OutsideSafeZone() => () =>
        {
            var distance = Vector3.Distance(transform.position, new Vector3(safeZone.transform.position.x,transform.position.y,safeZone.transform.position.y));
            return (distance >= safeZone.bounds.size.x/2);
        };
        
        Func<bool> InsideSafeZone() => () =>
        {
            var distance = Vector3.Distance(transform.position, new Vector3(safeZone.transform.position.x,transform.position.y,safeZone.transform.position.y));
            return (distance < safeZone.bounds.size.x/2);
        };
        
    }
    
    private void OnDisable()
    {
        Projectile.onHitPlayer -= Hit;
    }
    
    
}
