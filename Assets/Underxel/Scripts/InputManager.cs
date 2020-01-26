using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float moveTheshold = 0.2f;
    [Space]
    [SerializeField, Range(0, 10)] private float mouseSensitivity = 5;
    [SerializeField] private bool holdToRun = false;
    [SerializeField] private bool holdToCrouch = false;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    private BoolVariable isRunning;
    private BoolVariable isCrouching;

    public float MouseSensitivity => mouseSensitivity;
    public float Horizontal => Mathf.Abs(Input.GetAxis("Horizontal")) > moveTheshold ? Input.GetAxis("Horizontal") : 0;
    public float Vertical => Mathf.Abs(Input.GetAxis("Vertical")) > moveTheshold ? Input.GetAxis("Vertical") : 0;

    public bool IsRunning => isRunning.Value;
    public bool IsCrouching => isCrouching.Value;

    public bool HasInput => Horizontal != 0 || Vertical != 0;

    private void Start()
    {
        //isRunning.OnValueChanged += OnRunChange;
        //isCrouching.OnValueChanged += OnCrouchChange;
    }

    private void Update()
    {
        HandleRunningInput();

        if (!IsRunning)
            HandleCrouchingInput();
    }

    private void HandleRunningInput()
    {
        if (holdToRun)
        {
            if (Input.GetKeyDown(runKey)) isRunning.Value = true;
            else if (Input.GetKeyUp(runKey)) isRunning.Value = false;
        }
        else
        {
            if (Input.GetKeyDown(runKey)) isRunning.Value = !isRunning.Value;
        }
    }

    private void HandleCrouchingInput()
    {
        if (holdToCrouch)
        {
            if (Input.GetKeyDown(crouchKey)) isCrouching.Value = true;
            else if (Input.GetKeyUp(crouchKey)) isCrouching.Value = false;
        }
        else
        {
            if (Input.GetKeyDown(crouchKey)) isCrouching.Value = !isCrouching.Value;
        }
    }

    private void OnRunChange(bool value)
    {
        Debug.Log("run comes to: " + value);
    }

    private void OnCrouchChange(bool value)
    {
        Debug.Log("crouch comes to: " + value);
    }
}
