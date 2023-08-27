using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        if (StateContext.Instance.GetCurrentState is State.Playmode)
            transform.position = target.position + offset;
    }
}