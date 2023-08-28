using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveableButton : MonoBehaviour, IMovable
{
    [SerializeField] private GameObject targetObject;
    public Vector3 targetPos;
    public void Execute()
    {
        LeanTween.move(targetObject,targetPos,1.5f);
    }
}