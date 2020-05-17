using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float smoothAmount = 1;

    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Camera cam = null;
    private Ray ray = new Ray();
    private Vector3 mouseWorldPos = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;

        if (!cam)
        {
            Debug.LogWarning("камера с тегом MainCamera не найдена");
            Debug.Break();
        }
    }

    private void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out var enter)) mouseWorldPos = ray.GetPoint(enter);
    }

    private void FixedUpdate()
    {
        //var lerp = Vector3.Lerp(transform.position, mouseWorldPos, smoothAmount);
        var damp = Vector3.SmoothDamp(transform.position, mouseWorldPos, ref smoothVelocity, smoothAmount);
        transform.position = damp;
    }
}
