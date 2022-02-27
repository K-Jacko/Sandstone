using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRTwoHandGrabInteractable : Weapon
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    public enum TwoHandRotationType {None, First, Second};
    public TwoHandRotationType twoHandRotationType;
    private XRBaseInteractor secondHand;
    private Quaternion attachInitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondHand && selectingInteractor)
        {
            //Compute
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    public Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;

        if(twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(selectingInteractor.attachTransform.position - secondHand.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondHand.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(selectingInteractor.attachTransform.position - secondHand.attachTransform.position, secondHand.attachTransform.up);
        }
        
        return targetRotation;
    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Second Hand Grab");
        secondHand = args.interactor;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        Debug.Log("Second Hand LetGo");
        secondHand = null;
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        Debug.Log("First Hand Grab");
        base.OnSelectEntering(args);
        attachInitialRotation = args.interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        Debug.Log("First Hand LetGo");
        base.OnSelectExiting(args);
        secondHand = null;
        args.interactor.attachTransform.localRotation = attachInitialRotation;
    }
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
