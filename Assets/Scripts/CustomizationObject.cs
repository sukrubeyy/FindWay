using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationObject : Singleton<CustomizationObject>
{
    public Renderer BodyRenderer;
    public Renderer Eyesenderer;
    public List<Renderer> ArmsRenderer;

    public void Initialize()
    {
        BodyRenderer.material.color = DataManager.Instance.userInformation.GetBodyColor;
        Eyesenderer.material.color = DataManager.Instance.userInformation.GetEyesColor;
        foreach (var arm in ArmsRenderer)
            arm.material.color = DataManager.Instance.userInformation.GetArmsColor;
    }

    public void SetBodyColor(Color32 value)
    {
        BodyRenderer.material.color = value;
        DataManager.Instance.SetCustomizationColor(CustomizationButtonType.Body,value);
    }

    public void SetEyesColor(Color32 value)
    {
        Eyesenderer.material.color = value;
        DataManager.Instance.SetCustomizationColor(CustomizationButtonType.Eyes,value);
    }

    public void SetArmsColor(Color32 value)
    {
        foreach (var arm in ArmsRenderer)
            arm.material.color = value;
        
        DataManager.Instance.SetCustomizationColor(CustomizationButtonType.Arms,value);
    }
}