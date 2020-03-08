using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetTest : MonoBehaviour
{
    [SerializeField] private int maxCount = 3;

    private void FixedUpdate()
    {
        Gena(transform.position, transform.forward);
    }

    private void Gena(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < maxCount; i++)
        {
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(position, hit.point, Color.green);
                position = hit.point;
                //direction = hit.normal;
                direction = Vector3.Reflect(direction, hit.normal);
            }
            else
            {
                Debug.DrawRay(position, direction * 5, Color.red);
                break;
            }
        }
    }
}
