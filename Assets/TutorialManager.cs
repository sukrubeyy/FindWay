using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] PopUps;
    private int index = 0;
    public float animationDuration = 1.0f;

    public float opacityValue = 1f;
    
    private void Start()
    {
        OpenPopUp();
    }

    void Update()
    {
        if (MovementPopUp() && index == 0)
        {
            index++;
            OpenPopUp();
        }
        else if (ThrowPopUp() && index == 1)
        {
            index++;
            OpenPopUp();
        }
        else if (DashPopUp() && index == 2)
        {
            index++;
            OpenPopUp();
        }
        
        if(index==3)
            Debug.Log("Destroy Tutorial Manager");
    }

    void OpenPopUp()
    {
        for (int i = 0; i < PopUps.Length; i++)
        {
            if (i == index)
            {
                PopUps[i].SetActive(true);
                
                //Alpha Animation
                
                /*var imageComponent = PopUps[i].GetComponent<Image>();
                Color startColor = imageComponent.color;
                startColor.a = 0.0f;
                imageComponent.color = startColor;
        
               
                LeanTween.value(gameObject, 0.0f, opacityValue, animationDuration)
                    .setOnUpdate((float value) =>
                    {
                        Color newColor = imageComponent.color;
                        newColor.a = value;
                        imageComponent.color = newColor;
                    });*/
            }
            else
            {
                PopUps[i].SetActive(false);
            }
        }
    }

    private bool DashPopUp()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            return true;
        return false;
    }

    private bool ThrowPopUp()
    {
        if (Input.GetButtonDown("Fire1") && IsHitTicableObject())
            return true;

        return false;
    }

    private bool IsHitTicableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            if (hit.collider.GetComponent<ITicable>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private bool MovementPopUp()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)
           )
            return true;

        return false;
    }
}