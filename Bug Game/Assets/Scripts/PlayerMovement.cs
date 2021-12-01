using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;
    public GrapplingTongue grapple;
    public CameraController cameraController;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float jump = 3f;
    public float gravity = -9.81f;
    public float groundDistance = 0.5f;
    public float turnSmoothTime = 0.1f;
    
    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator anim;
    private float currentSpeed;

    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if ((isGrounded || grapple.isGrappling) && velocity.y < 0) {
            velocity.y = -2f;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (!grapple.isGrappling) {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                Run();
            } else {
                Walk();
            }

            if(cameraController.isAiming) {
                if (vertical == 0) {
                    anim.SetFloat("moveX", horizontal, 0.1f, Time.deltaTime);
                    anim.SetFloat("moveY", 0, 0.1f, Time.deltaTime);
                }
                else {
                    anim.SetFloat("moveX", 0, 0.1f, Time.deltaTime);
                    anim.SetFloat("moveY", vertical, 0.1f, Time.deltaTime);
                }
            } else {
                if(horizontal + vertical != 0) {
                    anim.SetFloat("moveX", 0, 0.1f, Time.deltaTime);
                    anim.SetFloat("moveY", 1, 0.1f, Time.deltaTime);
                }
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CameraController.currentCam.eulerAngles.y;

            if (!cameraController.isAiming) {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        } else {
            Idle();
        }

        if (isGrounded) {
            if (!grapple.isGrappling) {
                grapple.grappleMomentum = 0f;
            }
            if (Input.GetButtonDown("Jump")) {
                Jump();
            }
        }

        velocity.y += gravity * Time.deltaTime;
        if (!grapple.isGrappling) {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Idle() {
        anim.SetFloat("moveX", 0, 0.1f, Time.deltaTime);
        anim.SetFloat("moveY", 0, 0.1f, Time.deltaTime);
    }

    private void Walk() {
        currentSpeed = walkSpeed;
    }

    private void Run() {
        currentSpeed = runSpeed;
    }

    private void Jump() {
        velocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
        anim.SetTrigger("Jump");
    }
}
