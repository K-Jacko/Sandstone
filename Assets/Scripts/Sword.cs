using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sword : Weapon
{
    public GameObject wristSpring;
    Transform sword;
    MeshCollider blade;
    GameObject go;

    protected override void Awake()
    {
        sword = gameObject.transform;
        weaponType = WeaponType.Melee;
        base.Awake();
    }

    public void AttatchWristSpring(SelectEnterEventArgs args)
    {
        go = Instantiate(wristSpring, new Vector3(), new Quaternion(), args.interactor.transform);
        go.GetComponent<ConfigurableJoint>().connectedBody = sword.GetComponent<Rigidbody>();
        sword.transform.SetParent(go.transform);
    }


    // public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    // {
    //         go.transform.rotation = sword.rotation;
    //         go.transform.position = sword.position;
    
    //     base.ProcessInteractable(updatePhase);
    // }
}
