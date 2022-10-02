using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : Entity
{
    public enum PlayerState{Safe,Combat,Home}
    public PlayerState playerState = PlayerState.Home;
    
    private bool inSafeZone;
    private bool inHomeZone;
    private StateMachine _stateMachine;
    private ActionBasedContinuousMoveProvider _moveProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        Projectile.onHitPlayer += Hit;
        _stateMachine = new StateMachine();
        InitAttackStates();
        Init();

        castRefference.action.performed += Sprint;
        
    }

    void Init()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
        if (castRefference.action.WasReleasedThisFrame())
            _moveProvider.moveSpeed = 5;
    }

    void Hit()
    {
        Stamina -= 1;
    }
    
    void InitAttackStates()
    {
        var safe = new Safe(this);
        var combat = new Combat(this);
        var home = new Home();
        
        _stateMachine.AddAnyTransition(safe,  () => inSafeZone && !inHomeZone);
        _stateMachine.AddAnyTransition(combat,  () => !inSafeZone);
        _stateMachine.SetState(safe);
    }

    private void Sprint(InputAction.CallbackContext obj)
    {
        if(!_moveProvider) 
            _moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        _moveProvider.moveSpeed = Mathf.Lerp(_moveProvider.moveSpeed, _moveProvider.moveSpeed + Agility,2);
        
        Debug.Log("Sprinting");
    }
    
    private void OnDisable()
    {
        Projectile.onHitPlayer -= Hit;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("SafeZone"))
            inSafeZone = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SafeZone"))
            inSafeZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inSafeZone = false;
    }
}
