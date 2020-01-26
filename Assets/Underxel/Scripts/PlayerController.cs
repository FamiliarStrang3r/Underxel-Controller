using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underxel
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        private class SpeedSettings
        {
            public float movingSpeed = 0;
            public float soundDelay = 0;
        }

        [Header("Run, walk, crouch, stand")]
        [SerializeField] private SpeedSettings[] speedSetting = null;
        private SpeedSettings currentSpeed = null;

        [SerializeField] private LayerMask groundMask = 0;
        [SerializeField, Range(0, 20)] private float jumpForce = 5;
        [SerializeField, Range(0, 10)] private float gravityMultiplier = 2;
        [SerializeField] private float turnSpeed = 0.1f;
        [SerializeField] private AudioClip[] footstepSounds = null;

        private float groundCheckSize = 0.4f;
        private bool isGrounded = false;
        private Vector3 camForward = Vector3.zero;
        private Quaternion lookDirection = Quaternion.identity;

        private Vector3 rotationDirection = Vector3.zero;
        private Vector3 finalMoveVector = Vector3.zero;
        private Vector3 moveDir = Vector3.zero;

        private PlayerCamera playerCam = null;
        private Transform cam = null;
        private Animator anim = null;
        private CharacterController characterController = null;
        private InputManager inputManager = null;
        private AudioSource audioSource = null;

        private float forward = 0;
        private float turn = 0;
        //private float runCycleLegOffset = 0.2f;
        private Vector3 m_GroundNormal = Vector3.up;

        private float distanceTravelled = 0;

        private void Start()
        {
            playerCam = FindObjectOfType<PlayerCamera>();
            cam = playerCam.transform;

            anim = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            inputManager = FindObjectOfType<InputManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            GetInput();
            ConvertAnimator();
            HandleAnimator();
            HandleMoving();
        }

        private void FixedUpdate()
        {
            HandleRotation();
        }

        private SpeedSettings GetCurrentSpeed()
        {
            if (inputManager.IsRunning)
            {
                return speedSetting[0];
            }
            else
            {
                if (inputManager.IsCrouching) return speedSetting[2];
                else if (inputManager.HasInput) return speedSetting[1];
                else return speedSetting[3];
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 a = transform.position;
            Vector3 b = transform.position + Vector3.up;

            //Gizmos.DrawWireSphere(a, groundSphereRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(a, a + finalMoveVector);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(b, b + rotationDirection);

            //Gizmos.DrawWireSphere(a + camForward, groundSphereRadius);
        }

        private void GetInput()
        {
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            finalMoveVector = inputManager.Horizontal * cam.right + inputManager.Vertical * camForward;
        }

        private void HandleRotation()
        {
            if (finalMoveVector != Vector3.zero) rotationDirection = finalMoveVector;
            if (rotationDirection != Vector3.zero) lookDirection = Quaternion.LookRotation(rotationDirection);
            Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, lookDirection, turnSpeed);
            transform.rotation = smoothRotation;
        }

        private void ConvertAnimator()
        {
            Vector3 move = finalMoveVector.normalized;
            if (!inputManager.IsRunning && !inputManager.IsCrouching) move *= 0.5f;

            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            turn = Mathf.Atan2(move.x, move.z);
            forward = move.z;
        }

        private void CheckGroundStatus()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundCheckSize, groundMask);
            //Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out var hit, 0.3f, );
        }

        private void HandleAnimator()
        {
            anim.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
            anim.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
            anim.SetBool("Crouch", inputManager.IsCrouching);
            anim.SetBool("OnGround", isGrounded);

            if (!isGrounded) anim.SetFloat("Jump", characterController.velocity.y);

            //float runCycle = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            //float jumpLeg = (runCycle < 0.5f ? 1 : -1) * forward;
            //if (isGrounded) anim.SetFloat("JumpLeg", jumpLeg);
        }

        private void HandleMoving()
        {
            currentSpeed = GetCurrentSpeed();

            if (characterController.isGrounded)
            {
                distanceTravelled += characterController.velocity.magnitude * Time.deltaTime * currentSpeed.soundDelay;
                if(distanceTravelled >= 1)
                {
                    distanceTravelled = 0;
                    PlayFootStepAudio();
                }

                moveDir = finalMoveVector.normalized * currentSpeed.movingSpeed;
                if (Input.GetKeyDown(KeyCode.Space)) moveDir.y = jumpForce;
            }

            moveDir.y -= gravityMultiplier * -Physics.gravity.y * Time.deltaTime;
            characterController.Move(moveDir * Time.deltaTime);
        }

        public void PlayFootStepAudio()
        {
            if (isGrounded)
            {
                int index = Random.Range(1, footstepSounds.Length);
                audioSource.clip = footstepSounds[index];
                audioSource.PlayOneShot(audioSource.clip);

                footstepSounds[index] = footstepSounds[0];
                footstepSounds[0] = audioSource.clip;
            }
        }
    }
}
