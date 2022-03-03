using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            other.gameObject.GetComponent<Player>().Health -= 1;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject,2f);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,0f,0f,0.8f);
        var position = gameObject.transform.position;
        Gizmos.DrawSphere(position, explosionRadius);
    }
}