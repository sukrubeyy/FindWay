using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public class CharacterCustomization : MonoBehaviour
{
    public CustomizationSettings _customizationSettings;
    public Renderer EyesRenderer;
    public Renderer BodyRenderer;
    public List<Renderer> ArmsRenderer;
    
    private void Start()
    {
        if (File.Exists(PathHelper.Path.CustomizationFolderPath+PathHelper.FileName.CustomizationJsonName))
        {
            string path = PathHelper.Path.CustomizationFolderPath + PathHelper.FileName.CustomizationJsonName;
            string json = File.ReadAllText(path);
            _customizationSettings = JsonUtility.FromJson<CustomizationSettings>(json);
        }
        else
        {
            _customizationSettings.Initialize();
        }
         
        EyesRenderer.material.color = _customizationSettings.EyesColor;
        BodyRenderer.material.color = _customizationSettings.BodyColor;
        foreach (var arm in ArmsRenderer)
        {
            arm.material.color = _customizationSettings.ArmsColor;
        }
        
    }
}
