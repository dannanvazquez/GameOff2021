using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongue : MonoBehaviour {
    public CharacterController controller;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask grappleableMask;
    public Transform tongue, player;
    public float maxDistance = 20f;
    public float grappleMomentum = 0f;
    public bool isGrappling;
    public Animator anim;

    private float grappleMinSpeed = 5f;
    private float grappleMaxSpeed = 25f;
    private float initialGrappleDistance = 0f;
    private Vector3 grappleDirection = Vector3.zero;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && PauseMenuController.isPlaying) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0) && PauseMenuController.isPlaying) {
            StopGrapple();
        }

        
        Vector3 offset = grapplePoint - player.position;
        if (isGrappling) {
            float distanceRatio = initialGrappleDistance / Mathf.PI;
            grappleMomentum = Mathf.Max(Mathf.Sin(offset.magnitude / distanceRatio) * grappleMaxSpeed, grappleMinSpeed);
        } 
        else {
            grappleMomentum = Mathf.Max(grappleMomentum - .01f, 0);
        }
        if (grappleMomentum > 0) {
            if (offset.magnitude > 0.1f) {
                controller.Move(grappleDirection.normalized * grappleMomentum * Time.deltaTime);
            } else {
                grappleMomentum = 0f;
            }
        }
    }

    private void LateUpdate() {
        DrawTongue();
    }

    void StartGrapple() {
        //Calculations to find side c(Distance between camera to max distance) giving side a(Distance between camera and player), side b(Distance between player and max distance), and angle B(camera).
        float sideA = Vector3.Distance(CameraController.currentCam.transform.position, transform.position);
        float sideB = maxDistance;
        float angleB = Vector3.Angle(CameraController.currentCam.transform.position - CameraController.currentCam.TransformDirection(Vector3.forward) * 10, CameraController.currentCam.transform.position - transform.position) * Mathf.Deg2Rad;

        float angleA = Mathf.Asin(sideA * Mathf.Sin(angleB) / sideB);
        float angleC = 180 * Mathf.Deg2Rad - angleB - angleA;
        float camMaxDistance = sideB * Mathf.Sin(angleC) / Mathf.Sin(angleB);

        RaycastHit hit;
        if (Physics.Raycast(CameraController.currentCam.position, CameraController.currentCam.forward, out hit, camMaxDistance, grappleableMask)) {
            grapplePoint = hit.point;
            grappleDirection = grapplePoint - player.position;
            initialGrappleDistance = grappleDirection.magnitude;
            isGrappling = true;
            lr.positionCount = 2;
            anim.SetBool("IsGrappling", true);
            Debug.Log("GRAPPLE");
        }
    }

    void DrawTongue() {
        if (!isGrappling) return;
        lr.SetPosition(0, tongue.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple() {
        lr.positionCount = 0;
        isGrappling = false;
        anim.SetBool("IsGrappling", false);
    }
}
