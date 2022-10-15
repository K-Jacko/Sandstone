using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Prop Card")]
public class PropCard : ScriptableObject
{
    public int creditCost;
    public GameObject prop;
}
