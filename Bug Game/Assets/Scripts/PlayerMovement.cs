using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;
    public GrapplingTongue grapple;
    public CameraController cameraController;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float speed = 6f;
    public float runSpeed = 9f;
    public float jump = 3f;
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if ((isGrounded || grapple.isGrappling) && velocity.y < 0) {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            if(cameraController.isAiming) { 
}
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            if (!cameraController.isAiming) {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift)) {
                controller.Move(moveDir.normalized * runSpeed * Time.deltaTime);
            }
            else {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        if (!grapple.isGrappling) {
            controller.Move(velocity * Time.deltaTime);
        }

        if (cameraController.isAiming) {
            transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        }
    }
}
