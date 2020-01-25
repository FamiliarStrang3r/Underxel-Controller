using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underxel
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float jumpForce = 5;
        [SerializeField, Range(0, 5)] private float gravityMultiplier = 2;
        [SerializeField] private float turnSpeed = 0.1f;
        [SerializeField] private AudioClip[] footstepSounds = null;

        private bool isGrounded = false;
        private Vector3 camForward = Vector3.zero;
        private Quaternion lookDirection = Quaternion.identity;

        private Vector3 rotationDirection = Vector3.zero;
        private Vector3 finalMoveVector = Vector3.zero;

        private PlayerCamera playerCam = null;
        private Transform cam = null;
        private Animator anim = null;
        private Rigidbody rb = null;
        private InputManager inputManager = null;
        private AudioSource audioSource = null;

        private float forward = 0;
        private float turn = 0;
        //private float runCycleLegOffset = 0.2f;
        private Vector3 m_GroundNormal = Vector3.zero;

        private void Start()
        {
            playerCam = FindObjectOfType<PlayerCamera>();
            cam = playerCam.transform;

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            inputManager = FindObjectOfType<InputManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            GetInput();
            Move();
        }

        private void FixedUpdate()
        {
            HandleRotation();
        }

        public void OnAnimatorMove()
        {
            if (isGrounded && Time.deltaTime > 0)
            {
                Vector3 v = anim.deltaPosition / Time.deltaTime;
                v.y = rb.velocity.y;
                rb.velocity = v;
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

        private void Move()
        {
            Vector3 move = finalMoveVector.normalized;
            if (!inputManager.IsRunning && !inputManager.IsCrouching) move *= 0.5f;

            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            turn = Mathf.Atan2(move.x, move.z);
            forward = move.z;

            HandleAnimator();

            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 v = rb.velocity;
                    v.y = jumpForce;
                    rb.velocity = v;
                }
            }
            else
            {
                Vector3 force = (Physics.gravity * gravityMultiplier) - Physics.gravity;
                rb.AddForce(force);
            }
        }

        private void CheckGroundStatus()
        {
            isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out var hit, 0.3f);
            m_GroundNormal = isGrounded ? hit.normal : Vector3.up;
        }

        private void HandleAnimator()
        {
            anim.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
            anim.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
            anim.SetBool("Crouch", inputManager.IsCrouching);
            anim.SetBool("OnGround", isGrounded);

            if (!isGrounded) anim.SetFloat("Jump", rb.velocity.y);

            //float runCycle = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            //float jumpLeg = (runCycle < 0.5f ? 1 : -1) * forward;
            //if (isGrounded) anim.SetFloat("JumpLeg", jumpLeg);
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
