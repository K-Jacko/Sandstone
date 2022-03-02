using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : Monster
{
    public int GemType;
    public float explosionRadius;
    public float rotationSpeed = 80.0f;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float maxHeight;
    public float shootInterval = 1f;

    private GemElement _gemElement;
    private StateMachine _stateMachine;
    private Material _spawnerMaterial;
    private LayerMask _floor;
    private StageDirector _stageDirector;

    private bool _shooting;

    // Start is called before the first frame update
    void Awake()
    {
        player = StageDirector.GetPlayer();
        monster = gameObject;
        spawnPoint = transform.position;
        _floor = LayerMask.GetMask("Floor");
        _spawnerMaterial = GetComponent<MeshRenderer>().material;
        _stateMachine = new StateMachine();
        _stageDirector = FindObjectOfType<StageDirector>();
        _gemElement = _stageDirector.GetTopWeightedElement();
        GemType = _gemElement.GetElement();
        InitAttackStates();
        

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
        Move();
        if(!_shooting)
            StartCoroutine(Shoot());
    }

    void Move()
    {
        var position = player.transform.position;
        transform.RotateAround (position, Vector3.up, rotationSpeed * Time.deltaTime);
        Vector3  desiredPosition;
        if (_gemElement.GetElement() == 1)
        {
            //Above Head
            desiredPosition = (transform.position - position).normalized * -radius + position  ;
        }
        else if(_gemElement.GetElement() == 2)
        {
            //Rotate Around
            desiredPosition = ((transform.position - position).normalized * radius + position) * Mathf.Sin(1); 
        }
        else
        {
            //Weird Away Above
            desiredPosition = Vector3.zero;
        }

        var distanceFromFloor = GetDistanceFromFloor();
        var warp = new Vector3(desiredPosition.x, distanceFromFloor, desiredPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, warp, Time.deltaTime * radiusSpeed);
        transform.LookAt(player.transform);
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootInterval);
        _shooting = false;
    }
    float GetDistanceFromFloor()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 100f, _floor))
        {
            return hit.transform.position.y + maxHeight;
        }
        return maxHeight;
        
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

