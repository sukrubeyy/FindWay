using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<ITicable>()?.Execute();
        StartCoroutine(CoroutineSendPool());
    }

    private IEnumerator CoroutineSendPool()
    {
        yield return new WaitForSeconds(0.5f);
        PoolManager.Instance.SendPool(PoolObjectType.Stone, gameObject);
    }
}