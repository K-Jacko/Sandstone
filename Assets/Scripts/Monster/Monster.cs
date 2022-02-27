using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterState{Idle, Engaged}
    public MonsterState monsterState = MonsterState.Idle;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public MonsterSpawner spawner;

    public MonsterConfig conf;

    private Vector3 wanderTarget;
    // Start is called before the first frame update
    void Start()
    {
        spawner = gameObject.GetComponentInParent<MonsterSpawner>();
        conf = spawner.GetComponentInParent<MonsterConfig>();

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
    }
    
    void Update()
    {
        switch (monsterState)
        {
            case(MonsterState.Idle) :
                Wander();
                break;
            case(MonsterState.Engaged) :
                Attack();
                break;
        }
        
    }
    
    void Wander()
    {
        
        
    }

    Vector3 Cohesion()
    {
        //collect all and group
        return Vector3.negativeInfinity;
    }
    
    IEnumerator PrimeAttack()
    {
        yield return new WaitForSeconds(conf.attackInterval);
        monsterState = MonsterState.Engaged;
    }
    
    protected void Attack()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position,
            spawner.friendlies[0].gameObject.transform.position, conf.attackSpeed / 100);
    }
    
    void Seperation()
    {
        
    }

    public void Kill(GameObject go)
    {
        Destroy(go);
    }
}
