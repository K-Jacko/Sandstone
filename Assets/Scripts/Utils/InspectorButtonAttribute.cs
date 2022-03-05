using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public bool PlayModeOnly { get; private set; }
    public bool IsHoldButton { get; private set; }
    public string TextStr { get; private set; }

    public InspectorButtonAttribute() : this(false, false) { }
    public InspectorButtonAttribute(bool playModeOnly, bool isHoldButton, string textStr = null)
    {
        PlayModeOnly = playModeOnly;
        IsHoldButton = isHoldButton;
        TextStr = textStr;
    }
}
