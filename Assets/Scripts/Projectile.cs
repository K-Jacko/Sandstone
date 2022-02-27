using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5.0f;
    
    private Rigidbody thisRigidbody = null;

    private float projectileVelocity = 10;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        SetInnactive();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        SetInnactive();
    }
*/
    public void Launch(Weapon weapon)
    {
        //Position
        transform.position = weapon.firePoint.position;
        transform.rotation = weapon.firePoint.rotation;

        //Activate
        gameObject.SetActive(true);
        //Fire Track
        thisRigidbody.AddRelativeForce(Vector3.forward * weapon.Force, ForceMode.Impulse);
        StartCoroutine(TrackedLifetime());
    }

    private IEnumerator TrackedLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        SetInnactive();
    }
    public void SetInnactive()
    {
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }
}
