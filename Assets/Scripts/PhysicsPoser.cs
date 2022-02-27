using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRBaseController))]
public class PhysicsPoser : MonoBehaviour
{
    public float physicsRange = 0.1f;
    public LayerMask phsicsMask = 0;

    [Range(0,1)] public float slowDownVelocity = 0.75f;
    [Range(0,1)] public float slowDownAngularVelocity = 0.75f;

    [Range(0, 100)] public float maxPositionChange = 75.0f;
    [Range(0,100)] public float maxRotationChange = 75.0f;

    private Rigidbody rigidbody = null;
    public XRController controller;
    private XRBaseInteractor interactor = null;

    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRoataion = Quaternion.identity;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<XRController>();
        interactor = GetComponent<XRBaseInteractor>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateTracking(controller.inputDevice);
        MoveUsingTransform();
        RotateUsingTransform();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTracking(controller.inputDevice);
    }

    private void UpdateTracking(InputDevice inputDevice)
    {
        inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out targetPosition);
        inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out targetRoataion);
    }

    private void FixedUpdate()
    {
        if(IsHoldingObject() || !WithinPhysicsRange())
        {
            MoveUsingTransform();
            RotateUsingTransform();
        }

        else{
            MoveUsingPhysics();
            RotateUsingPhysics();
        }
    }

    private bool IsHoldingObject()
    {
        return interactor.selectTarget;
    }

    public bool WithinPhysicsRange()
    {
        return Physics.CheckSphere(transform.position, physicsRange, phsicsMask, QueryTriggerInteraction.Ignore);
    }

    private void MoveUsingPhysics()
    {
        rigidbody.velocity *= slowDownVelocity;

        Vector3 velocity = FindNewVelocity();

        if(IsValidVelocity(velocity.x))
        {
            float maxChange = maxPositionChange * Time.deltaTime;
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, velocity, maxChange);
        }
    }

    private Vector3 FindNewVelocity()
    {
        // Figure out the difference we can move this frame
        Vector3 worldPosition = transform.root.TransformPoint(targetPosition);
        Vector3 difference = worldPosition - rigidbody.position;
        return difference / Time.deltaTime;
    }

    private void RotateUsingPhysics()
    {
        rigidbody.angularVelocity *= slowDownAngularVelocity;

        Vector3 angularVelocity = FindNewAngularVelocity();

        if(IsValidVelocity(angularVelocity.x))
        {
            float maxChange = maxRotationChange * Time.deltaTime;
            rigidbody.angularVelocity = Vector3.MoveTowards(rigidbody.angularVelocity, angularVelocity, maxChange);
        }
    }

    private Vector3 FindNewAngularVelocity()
    {
        // Figure out the difference in rotation
        Quaternion worldRotation = transform.root.rotation * targetRoataion;
        Quaternion difference = worldRotation * Quaternion.Inverse(rigidbody.rotation);
        difference.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);

        // Do the weird thing to account for have a range of -180 to 180
        if (angleInDegrees > 180)
            angleInDegrees -= 360;

        // Figure out the difference we can move this frame
        return (rotationAxis * angleInDegrees * Mathf.Deg2Rad) / Time.deltaTime;
    }

    private bool IsValidVelocity(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    private void MoveUsingTransform()
    {
        rigidbody.velocity = Vector3.zero;
        transform.localPosition = targetPosition;
    }

    private void RotateUsingTransform()
    {
        rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = targetRoataion;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, physicsRange);
    }

    private void Onvalidate()
    {
        if(TryGetComponent(out Rigidbody rigidbody))
            rigidbody.useGravity = false;
    }
}
