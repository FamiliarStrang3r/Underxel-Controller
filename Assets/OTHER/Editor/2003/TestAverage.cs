using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAverage : MonoBehaviour
{
    public Vector3[] points = new Vector3[1];
    public Vector3 center;

    private void OnDrawGizmos()
    {
        var sum = Vector3.zero;

        Gizmos.color = Color.red;
        for (int i = 0; i < points.Length; i++)
        {
            sum += points[i];
            Gizmos.DrawSphere(points[i] + transform.position, 0.1f);
        }

        center = sum / points.Length;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center + transform.position, 0.1f);
    }
}
