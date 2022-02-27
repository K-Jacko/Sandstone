using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YValue : MonoBehaviour
{
    public float yValue;
    public static YValue instance;

    void Awake()
    {
        instance = this;
    }
}
