using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<PickableItem>();
        item?.StartMoving(transform);
    }
}
