using System;
using UnityEngine;

[Serializable]
public struct CustomizationSettings
{
    public Color32 EyesColor;
    public Color32 BodyColor;
    public Color32 ArmsColor;
    public void Initialize()
    {
        EyesColor = Color.red;
        BodyColor = Color.black;
        ArmsColor = Color.magenta;
    }
    public void SetEyesColor(Color32 value) => EyesColor = value;
    public void SetBodyColor(Color32 value) => BodyColor = value;
    public void SetArmsColor(Color32 value) => ArmsColor = value;
}