using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject aimReticle;
    public GameObject cameraLock;
    public GameObject mainCam;
    public GameObject thirdPersonCam;
    public bool isAiming;
    public LayerMask collisionMask;

    private float xRotation = 0f;
    public static float sensitivity = 150f;
    public static Transform currentCam;

    // Update is called once per frame
    void Update() {
        if (isAiming) {
            currentCam = thirdPersonCam.transform;
        }
        else {
            currentCam = mainCam.transform;
        }
        if (Input.GetMouseButtonDown(1) && PauseMenuController.isPlaying) { //When aiming
            mainCam.SetActive(false);
            thirdPersonCam.SetActive(true);

            mainCam.transform.position = new Vector3(0f, 0f, -5f);

            cameraLock.transform.localPosition = new Vector3(1.5f, 0.5f, 0);
            isAiming = true;

            aimReticle.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1) && PauseMenuController.isPlaying) { //When not aiming
            thirdPersonCam.SetActive(false);
            mainCam.SetActive(true);

            cameraLock.transform.localPosition = new Vector3(0f, 0.5f, 0);
            aimReticle.SetActive(false);
            isAiming = false;
        }

        if(isAiming) {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraLock.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            Debug.DrawRay(cameraLock.transform.position, -cameraLock.transform.forward * 5f, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(cameraLock.transform.position, -cameraLock.transform.forward, out hit, 6f, collisionMask)) {
                thirdPersonCam.transform.position = hit.point + cameraLock.transform.forward * 1f;
            }
            else {
                thirdPersonCam.transform.localPosition = new Vector3(0f, 0f, -5f);
            }
        }

    }
}