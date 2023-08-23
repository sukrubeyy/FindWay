using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Vector3 offset;
    private StateContext context;

    private void Start()
    {
        offset = transform.position - target.position;
        PlayerController controller = FindObjectOfType<PlayerController>();
        context = new StateContext(controller);
    }

    private void FixedUpdate()
    {
        if (context.GetCurrentState is not State.Playmode)
            transform.position = target.position + offset;
    }
}