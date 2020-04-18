using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private bool useTorque = true;
    [SerializeField] private float maxAngularVelocity = 5;

    [SerializeField] private float movePower = 5;
    [SerializeField, Range(0, 10)] private float jumpPower = 2;
    [SerializeField, Range(0, 1)] private float castRadius = 1;
    [SerializeField] private LayerMask mask = 0;

    private Rigidbody rb = null;
    private Transform cam = null;
    private bool isGrounded = false;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 castOrigin = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Camera.main не существует");
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        if (cam)
        {
            var camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            moveDirection = (horizontal * cam.right + vertical * camForward).normalized;
        }
        else
        {
            moveDirection = (horizontal * Vector3.right + vertical * Vector3.forward).normalized;
        }
    }

    private void FixedUpdate()
    {
        rb.maxAngularVelocity = maxAngularVelocity;

        if (useTorque)
        {
            var dir = new Vector3(moveDirection.z, 0, -moveDirection.x);
            rb.AddTorque(dir * movePower);
        }
        else
        {
            rb.AddForce(moveDirection * movePower);
        }

        castOrigin = transform.position + Vector3.down * 0.5f;
        isGrounded = Physics.CheckSphere(castOrigin, castRadius, mask);
    }

    private void ClampVelocity()
    {
        //var vel = rb.velocity;
        //var value = 10;
        //vel.x = Mathf.Clamp(vel.x, -value, value);
        //vel.z = Mathf.Clamp(vel.z, -value, value);
        //rb.velocity = vel;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(castOrigin, castRadius);
        }
    }

    private void OnGUI()
    {
        //var c = GUI.color;
        //GUI.color = Color.black;
        //var rect = new Rect(10, 10, 150, 30);
        //GUI.Label(rect, rb.velocity.ToString());
        //GUI.color = c;
    }
}
