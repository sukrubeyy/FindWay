using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 150)] public float _speed = 5f;
    [SerializeField] private Rigidbody rb;
    private Vector3 input;
    public bool isGrounded=true;
    public float rotateDegree = 360;
    public float throwForce = 10f;
    public GameObject stonePrefab;
    public Camera mainCamera;
    
    public float coyoTime=0.5f;
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * Time.deltaTime * _speed);
        // if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        //     Jump();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Look();

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.point != null)
                {
                    var direction =  hit.point - transform.position;
                    ThrowStone(direction);
                    Debug.DrawRay(transform.position, direction, Color.red);
                }
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    void ThrowStone(Vector3 throwPoint)
    {
         // Taşın kopyasını oluştur
         GameObject stone = Instantiate(stonePrefab, transform.position+Vector3.one, transform.rotation);
        
         // Taşa bir kuvvet uygula
         Rigidbody stoneRb = stone.GetComponent<Rigidbody>();
         if (stoneRb != null)
         {
             stoneRb.AddForce(throwPoint * throwForce, ForceMode.Impulse);
         }
    }

    void Look()
    {
        if (input == Vector3.zero) return;

        var matrix = Matrix4x4.Rotate(Quaternion.identity);
        var skewedInput = matrix.MultiplyPoint3x4(input);

        var relative = (transform.position + skewedInput) - transform.position;
        var rot = Quaternion.LookRotation(relative, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotateDegree * Time.deltaTime);
    }

    IEnumerator CoyoMechanism()
    {
        yield return new WaitForSeconds(coyoTime);
        isGrounded = false;
    }
}