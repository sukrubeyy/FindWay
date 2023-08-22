using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour, IFracturable
{
    private float throwForce = 30f;
    private Rigidbody rb;
    public void ExecuteFracture(Transform direction)
    {
        if (rb is null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        rb.AddForce(direction.forward * throwForce, ForceMode.Impulse);
        StartCoroutine(RemoveRigidbody());
        IEnumerator RemoveRigidbody()
        {
            yield return new WaitForSeconds(2f);
            GetComponent<BoxCollider>().isTrigger = true;
            Destroy(rb);
        }
    }
}