using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject aimReticle;
    public GameObject cameraLock;
    public bool isAiming;

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            cameraLock.transform.localPosition = new Vector3(1.5f, 0.5f, 0);
            isAiming = true;

            aimReticle.SetActive(enabled);
        }
        else if (Input.GetMouseButtonUp(1)) {
            cameraLock.transform.localPosition = new Vector3(0f, 0.5f, 0);
            aimReticle.SetActive(false);
            isAiming = false;
        }

    }
}
