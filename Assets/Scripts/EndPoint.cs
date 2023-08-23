using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public float radius;
    public Color radiusColor;
    [SerializeField] private Transform target;
    [SerializeField] private bool isFinish=false;
    private StateContext context;
    public GameManager gameManager;
    private void Start()
    {
        PlayerController controller = FindObjectOfType<PlayerController>();
        context = new StateContext(controller);
    }

    private void OnDrawGizmos()
    {
        Handles.color = radiusColor;
        Handles.DrawWireDisc(transform.position,Vector3.up,radius,3f);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= radius && !isFinish)
        {
            context.Transition(State.WinState);
            LeanTween.rotate(target.gameObject, transform.eulerAngles, 1f).setOnComplete(() =>
            {
                LeanTween.move(target.gameObject, transform.position, 1f).setOnComplete(OpenWinMenu);
                
            }).destroyOnComplete=false;
        }
    }

    private void OpenWinMenu()
    {
        isFinish = true;
        gameManager.FinishSuccess();
    }
}
