using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField, Range(0, 180)] private int maxAngle = 180;
    [SerializeField, Range(0, 1)] private float smoothSpeed = 1;
    [SerializeField] private int shootsPerSecond = 2;
    [SerializeField] private Transform shootPoint = null;
    [SerializeField] private Rigidbody bulletPrefab = null;
    [SerializeField] private float bulletSpeed = 5;

    public int MaxAngle => maxAngle;

    private bool isPressed = false;
    private Camera cam = null;
    private Plane plane = new Plane();
    private Vector3 tapPoint = Vector3.zero;
    private float lastTime = 0;

    private void Start()
    {
        cam = Camera.main;
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            //tapPoint = Vector3.zero;
        }

        if (isPressed)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var enter)) tapPoint = ray.GetPoint(enter);

            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Shoot()
    {
        if (Time.time > lastTime)
        {
            lastTime = Time.time + 1f / shootsPerSecond;

            var b = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            b.velocity = b.transform.forward * bulletSpeed;

            Destroy(b.gameObject, 3);
        }
    }

    private void Rotate()
    {
        Vector3 dir = tapPoint - transform.position;
        dir.y = 0;

        float angle = -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
        if (angle > 180) angle -= 360;

        angle = Mathf.Clamp(angle, -maxAngle / 2f, maxAngle / 2f);

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, smoothSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tapPoint, 0.2f);
    }
}
