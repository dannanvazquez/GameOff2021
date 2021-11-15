using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongue : MonoBehaviour {
    public CharacterController controller;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask grappleableMask;
    public Transform tongue, player;
    private Transform mainCamera;
    private float maxDistance = 20f;
    private SpringJoint joint;
    public bool isGrappling;

    private float grappleSpeed = 20f;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
        mainCamera = Camera.main.transform;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }

        if (isGrappling) {
            Vector3 offset = grapplePoint - player.position;
            if (offset.magnitude > 0.1f) {
                controller.Move(offset.normalized * grappleSpeed * Time.deltaTime);
            }
        }
    }

    private void LateUpdate() {
        DrawTongue();
    }

    void StartGrapple() {
        //Calculations to find side c(Distance between camera to max distance) giving side a(Distance between camera and player), side b(Distance between player and max distance), and angle B(camera).
        float sideA = Vector3.Distance(mainCamera.transform.position, transform.position);
        float sideB = maxDistance;
        float angleB = Vector3.Angle(mainCamera.transform.position - mainCamera.TransformDirection(Vector3.forward) * 10, mainCamera.transform.position - transform.position) * Mathf.Deg2Rad;

        float angleA = Mathf.Asin(sideA * Mathf.Sin(angleB) / sideB);
        float angleC = 180 * Mathf.Deg2Rad - angleB - angleA;
        float camMaxDistance = sideB * Mathf.Sin(angleC) / Mathf.Sin(angleB);

        Debug.Log("Distance: " + camMaxDistance);

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, camMaxDistance, grappleableMask)) {
            grapplePoint = hit.point;
            isGrappling = true;
            lr.positionCount = 2;
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
    }
}
