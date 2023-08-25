using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

public class ColorButton : MonoBehaviour
{
    public Color color;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private CustomizationButtonType type; 
   
    void Start()
    {
        image.color=color;
        button.onClick.AddListener(() =>
        {
            switch(type)
            {
                case CustomizationButtonType.Body:
                    CustomizationObject.Instance.SetBodyColor(color);
                    break;
                case CustomizationButtonType.Eyes:
                    CustomizationObject.Instance.SetEyesColor(color);
                    break;
                case CustomizationButtonType.Arms:
                    CustomizationObject.Instance.SetArmsColor(color);
                    break;
            }

            //TODO: UserInformation -> Color set operation
        });
    }

   
}



public enum CustomizationButtonType
{
    Body,
    Eyes,
    Arms
}
