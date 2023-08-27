using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 150)] public float _speed = 5f;
    [SerializeField] private Rigidbody rb;
    private Vector3 input;
    [SerializeField] private bool isGrounded = true;
    public float rotateDegree = 360;
    public float throwForce = 10f;
    public float coyoTime = 0.5f;

    [Header("Dash")] private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 30f;
    public float maxDashPower = 15f;
    private float dashTime = 0.2f;
    private float dashCoolDown = 1f;

    [Header("State Pattern")]
    private StateContext Context;


    public AudioManager _audioManager;
    public PoolManager poolManager;
    public GameManager gameManager;
    
    
    private float FireRate= 1.0f;
    private float nextFire = default;
    
        
    private void Start()
    {
        Context = new StateContext(this);
        Context.Transition(State.Playmode);
    }

    private void FixedUpdate()
    {
        if (Context.GetCurrentState is State.LoseState)
        {
            gameManager.LosePanelActive();
            Destroy(rb);
        }
        if (isDashing) return;
        if (Context.GetCurrentState is not State.Playmode)
            return;

        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * Time.deltaTime * _speed);
        
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (transform.position.y < -5f)
        {
            Context.Transition(State.LoseState);
        }
    }

    void Update()
    {
        if (isDashing) return;
        if(Context.GetCurrentState is not State.Playmode)
            return;
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Look();

        if (Input.GetButtonDown("Fire1") )
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.point != null)
                {
                    if (hit.collider.GetComponent<ITicable>() != null)
                    {
                        if (Time.time > nextFire)
                        {
                             nextFire = Time.time + FireRate;
                            var direction = hit.point - transform.position;
                            ThrowStone(direction);
                            Debug.DrawRay(transform.position, direction, Color.red);
                        }
                       
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashMechanism());
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        _audioManager.ExecuteClip(AudioClipType.Jump2);
    }

    void ThrowStone(Vector3 throwPoint)
    {
        GameObject stone = poolManager.GetPoolObject(PoolObjectType.Stone);
        stone.transform.position = transform.position + Vector3.one;
        stone.transform.rotation = transform.rotation;
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
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        if (isDashing)
            collision.gameObject.GetComponent<IFracturable>()?.ExecuteFracture(transform);
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        StartCoroutine(CoyoMechanism());
    }

    IEnumerator CoyoMechanism()
    {
        yield return new WaitForSeconds(coyoTime);
        isGrounded = false;
    }

    IEnumerator DashMechanism()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = transform.forward * dashingPower;
        rb.velocity = rb.velocity.magnitude > maxDashPower ? rb.velocity.normalized * maxDashPower : rb.velocity;
        _audioManager.ExecuteClip(AudioClipType.Dash);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}