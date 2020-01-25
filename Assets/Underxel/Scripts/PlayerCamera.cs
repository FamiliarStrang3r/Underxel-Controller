using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private Vector2 minMax = Vector2.zero;
	[SerializeField, Range(0, 1)] private float dampSpeed = 0;

    private Transform pivot = null;
    private Vector3 smoothInput = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;
    private float yaw = 0;
    private float pitch = 0;
    private InputManager inputManager = null;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        pivot = transform.GetChild(0);
    }

    //public void SetTarget(Transform newTarget) => target = newTarget;

    private void LateUpdate()
    {
        pivot.localPosition = offset;
        transform.position = target.position;
    }

    private void FixedUpdate()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 mouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        smoothInput.x = Mathf.SmoothDamp(smoothInput.x, mouseInput.x, ref smoothVelocity.x, dampSpeed);
        smoothInput.y = Mathf.SmoothDamp(smoothInput.y, mouseInput.y, ref smoothVelocity.y, dampSpeed);

        yaw += smoothInput.x * inputManager.MouseSensitivity;
        yaw %= 360;
        transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);

        pitch -= smoothInput.y * inputManager.MouseSensitivity;
        pitch = Mathf.Clamp(pitch, minMax.x, minMax.y);
        pivot.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
    }
}