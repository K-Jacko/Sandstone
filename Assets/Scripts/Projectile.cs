using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius;
    public static Action onHitPlayer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject,2f);
            onHitPlayer?.Invoke();
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