using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    [SerializeField] private bool drawGizmo = false;
	[SerializeField, Range(0, 5)] private float targetLength = 3;
    [SerializeField, Range(0, 1)] private float dampSpeed = 0;
	[SerializeField] private LayerMask obstacleMask = 0;
    private bool hitObstacle = false;

	private float camViewportExtentsMultipllier = 1;
	private float collisionRadius = 0;

	private Transform collisionSocket = null;
	private Camera cam = null;
    private Vector3 smoothVelocity = Vector3.zero;

    private void Start()
    {
        collisionSocket = transform.GetChild(0);
        cam = collisionSocket.GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
	{
		if (cam != null)
		{
			collisionRadius = GetCollisionRadiusForCamera(cam);
			cam.transform.localPosition = Vector3.back * cam.nearClipPlane;
		}

		UpdateLength();
	}

	private float GetCollisionRadiusForCamera(Camera cam)
	{
		float halfFOV = cam.fieldOfView / 2 * Mathf.Deg2Rad; // vertical FOV in radians
		float nearClipPlaneHalfHeight = Mathf.Tan(halfFOV) * cam.nearClipPlane * camViewportExtentsMultipllier;
		float nearClipPlaneHalfWidth = nearClipPlaneHalfHeight * cam.aspect;
		float radius = new Vector2(nearClipPlaneHalfWidth, nearClipPlaneHalfHeight).magnitude; // Pythagoras
		return radius;
	}

	private void UpdateLength()
	{
		float finalLength = GetDesiredTargetLength();
		Vector3 newSocketLocalPosition = Vector3.back * finalLength;
		collisionSocket.localPosition = Vector3.SmoothDamp(collisionSocket.localPosition, newSocketLocalPosition, ref smoothVelocity, dampSpeed);
	}

    private float GetDesiredTargetLength()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        hitObstacle = Physics.SphereCast(ray, Mathf.Max(0.001f, collisionRadius), out var hit, targetLength, obstacleMask);
        return hitObstacle ? hit.distance : targetLength;
    }

    private void OnDrawGizmos()
	{
        if (!Application.isPlaying) return;

        if (!collisionSocket || !drawGizmo) return;

        Gizmos.DrawWireSphere(collisionSocket.transform.position, collisionRadius);
		Gizmos.color = hitObstacle ? Color.red : Color.green;
		Gizmos.DrawLine(transform.position, collisionSocket.transform.position);
	}
}