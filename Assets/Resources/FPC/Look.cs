using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour {

    float x, y;
    public float sensitivity = 5;
    public Transform camera;
    float flickTime = 0, flickWaitTime = 200;
    public static bool enabled = true;

    void Start () {
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update () {
        if (Look.enabled) {
            x = Input.GetAxis ("Mouse X") * sensitivity;
            y = Input.GetAxis ("Mouse Y") * sensitivity;

            transform.Rotate (0, x, 0, Space.Self);
            Vector3 angles = camera.localEulerAngles;
            // Debug.Log("x " + angles.x);
            
            if (angles.x-y > 270 || angles.x-y < 90){
                camera.Rotate(-y, 0, 0, Space.Self);    
            }

            // if (flickTime <= 0) {
            //     if (Input.GetAxisRaw ("Fire1") != 0) {
            //         transform.Rotate (0, 180, 0, Space.Self);
            //         flickTime = flickWaitTime;
            //     }

            // } else {
            //     flickTime -= Time.deltaTime * 1000;

            // }

        }

    }

}