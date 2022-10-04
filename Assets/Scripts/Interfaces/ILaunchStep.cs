using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchStep
{
    public event Action onComplete;
    public void Init();
    public void DeInit();
    public bool IsRequired();
}
