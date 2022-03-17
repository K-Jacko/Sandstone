using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 5;
    public float distanceTickThreshold = 25f;

    public static Action OnPlayerDistanceTick;

    public static Vector2 PlayerPosition;

    private Vector2 _oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        Projectile.onHitPlayer += Hit;
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
    }

    void Hit()
    {
        health -= 1;
    }
    
    private void OnDisable()
    {
        Projectile.onHitPlayer -= Hit;
    }
    
}
