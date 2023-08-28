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

    [SerializeField] private Transform RockDashTransform;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private float rockDashRadius = 3f;

    private void Start()
    {
        OpenPopUp(index);
    }

    void Update()
    {
        if (StateContext.Instance.GetCurrentState is not State.Playmode)
        {
          Destroy(gameObject);
        }
        if (MovementPopUp() && index == 0)
        {
            ClosePopUp(index);
            index++;
            OpenPopUp(index);
        }
        else if (ThrowPopUp() && index == 1)
        {
            ClosePopUp(index);
           
        }
        else if (Vector3.Distance(PlayerTransform.position, RockDashTransform.position) <= rockDashRadius && index==1)
        {
            index++;
            OpenPopUp(index);
        }
        else if (DashPopUp() && index == 2)
        {
            ClosePopUp(index);
        }
    }

    void OpenPopUp(int popUpIndex)
    {
        if (popUpIndex > PopUps.Length - 1) return;
        PopUps[popUpIndex].SetActive(true);

        var childrenImageComponents = PopUps[popUpIndex].GetComponentsInChildren<Image>();
        foreach (var childrenImage in childrenImageComponents)
        {
            Color startChilderenColor = childrenImage.color;
            startChilderenColor.a = 0.0f;
            childrenImage.color = startChilderenColor;

            LeanTween.value(gameObject, 0.0f, 1, animationDuration)
                .setOnUpdate((float value) =>
                {
                    Color childrensNewColor = childrenImage.color;
                    childrensNewColor.a = value;
                    childrenImage.color = childrensNewColor;
                });
        }

        var imageComponent = PopUps[popUpIndex].GetComponent<Image>();
        Color startColor = imageComponent.color;
        startColor.a = 0.0f;
        imageComponent.color = startColor;


        LeanTween.value(gameObject, 0.0f, opacityValue, animationDuration)
            .setOnUpdate((float value) =>
            {
                Color newColor = imageComponent.color;
                newColor.a = value;
                imageComponent.color = newColor;
            });
    }

    void ClosePopUp(int popUpIndex)
    {
        var popup = PopUps[popUpIndex];

        var childrenImageComponents = popup.GetComponentsInChildren<Image>();

        foreach (var image in childrenImageComponents)
        {
            LeanTween.value(gameObject, 1, 0.0f, animationDuration)
                .setOnUpdate((float value) =>
                {
                    Color newColor = image.color;
                    newColor.a = value;
                    image.color = newColor;

                    if (value <= 0.0f)
                    {
                        popup.SetActive(false);
                        if(index==2)
                            Destroy(gameObject);
                    }
                });
        }


        var imageComponent = popup.GetComponent<Image>();

        LeanTween.value(gameObject, opacityValue, 0.0f, animationDuration)
            .setOnUpdate((float value) =>
            {
                Color newColor = imageComponent.color;
                newColor.a = value;
                imageComponent.color = newColor;

                if (value <= 0.0f)
                {
                    popup.SetActive(false);
                }
            });
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
            if (hit.collider.GetComponent<IMovable>() != null)
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