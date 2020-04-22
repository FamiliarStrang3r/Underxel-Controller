using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomebodyController : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float walkSpeed = 0;
    [SerializeField] private float crouchSpeed = 0;
    [SerializeField] private float runSpeed = 0;

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isCrouching = false;

    [SerializeField] private string horizontalAxis = string.Empty;
    [SerializeField] private string verticalAxis = string.Empty;

    private void Start()
    {
        walkSpeed = 0;
        crouchSpeed = 0;
        runSpeed = 0;
        isGrounded = false;
        isMoving = false;
        isCrouching = false;
    }
}
