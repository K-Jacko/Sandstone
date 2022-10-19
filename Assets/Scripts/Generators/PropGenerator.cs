using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PropGenerator : MonoBehaviour
{
    public Entity Player { get; private set; }
    public static PropGenerator Instance { get; private set; }
    public PropCard[] propCards;
    public int bigwallet;

    private PropCard firstSpent;
    private GameObject parentGameObject;

    private bool spent = true;
    // Start is called before the first frame update
    //Use Task action to make sure Everything load is right order
    public void Awake()
    {
        Init();
    }

    void Init()
    {
        LoadResources();
        parentGameObject = new GameObject("Props");
        parentGameObject.name = new string("Props");

    }
    
    public void GeneratePropOnGrid(SpawnNode spawnNode, int wallet, float spawnRadius)
    {
        
        //start on object
        //get location
        //spend credits
        //move on
        //if wallet hits 0 finish
        var nameIndex = 0;
        
        for (int i = 0; i < propCards.Length; i++)
        {
            
            if (wallet >= propCards[i].creditCost)
            {
                if (Physics.Raycast(spawnNode.raycastLocation + new Vector3(Random.Range(0f, spawnRadius),0,Random.Range(0f, spawnRadius)),Vector3.down,out RaycastHit hitInfo, 100, 1 << 9))
                {
                    var spawnRoatation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                    Collider[] collidersInOverlay = new Collider[1];
                    int numberOfCollidersFound =
                        Physics.OverlapBoxNonAlloc(hitInfo.point, propCards[i].prop.GetComponent<Collider>().bounds.extents, collidersInOverlay, spawnRoatation, 9);
                    if (numberOfCollidersFound == 0)  
                    {
                        var go = Instantiate(propCards[i].prop, hitInfo.point + new Vector3(0, propCards[i].prop.GetComponent<MeshRenderer>().bounds.extents.y,0),spawnRoatation);
                        spawnNode.props.Add(go);
                        go.name = new String($"{propCards[i].prop.name}" + $"{nameIndex}");
                        go.transform.parent = parentGameObject.transform;
                        wallet -= propCards[i].creditCost;
                        nameIndex++;
                        if (spent)
                        {
                            firstSpent = propCards[i];
                            if(wallet >= firstSpent.creditCost)
                            {
                                //Debug.Log("CanAffordFirst");
                                i--;
                            }  
                            spent = false;
                        }
                        
                    }
                    else
                    {
                        //Debug.Log("No valid surfaces found");
                    }
                }
                else
                {
                    //Debug.Log("MissedRaycast" + $"{spawnNode.raycastLocation}");
                    i--;
                    continue;
                }
            }
            else
            {
                //Debug.Log("Cannot Afford Prop");
            }
            
            if (wallet <= 0)
            {
                break;
            }

            if (!spent)
            {
                if(wallet >= firstSpent.creditCost)
                {
                    //Debug.Log("CanAffordFirst");
                    ;
                }
            }
            

            // if (wallet > propCards[^1].creditCost)
            // {
            //     Debug.Log($"{propCards[^1].creditCost}");
            //     i--;
            // }
            
            // else
            // {
            //     Debug.Log("Edge");
            // }
            
            
        }
        bigwallet = wallet;
        
        if (wallet >= propCards[^1].creditCost)
        {
            nameIndex++;
            GeneratePropOnGrid(spawnNode,wallet,spawnRadius);
        }

    }
    void LoadResources()
    {
        //Most expensive at 0index
        propCards = Resources.LoadAll<PropCard>("PropCards");
        Array.Sort(propCards,
            delegate(PropCard x, PropCard y) { return y.creditCost.CompareTo(x.creditCost); });
    }
    
}
