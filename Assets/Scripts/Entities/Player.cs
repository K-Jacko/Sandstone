using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 5;
    // Start is called before the first frame update
    void Start()
    {
        Projectile.onHitPlayer += Hit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hit()
    {
        health -= 1;
    }
}
