using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public Camera cam;
    GameObject hand;
    Vector3 startPos, oldLocalPos, oldAnchor;
    Quaternion handLocalRot;
    InvPlayer inv;
    float swingTime = 0, swingDur = 0.5f;
    float sens = 0.1f;
    Vector3 v;
    float xoff = 0, yoff = 0;
    Vector3 swingAxis;
    Transform oldParent;

    void Start () {
        startPos = Vector3.forward;
        v = new Vector3 ();
        inv = gameObject.GetComponent<InvPlayer> ();
        
        hand = inv.handsAnchorR;
        oldLocalPos = hand.transform.localPosition;
        oldParent = hand.transform.parent;
        ConfigurableJoint cj = hand.GetComponent<ConfigurableJoint> ();
        oldAnchor = cj.anchor;

    }

    void Update () {
        if (swingTime == 0) {
            if (Input.GetKeyDown (KeyCode.V)) {

                // place the hand one unit away from the camera
                hand.transform.SetParent (cam.transform);
                hand.transform.localPosition = startPos;
                ConfigurableJoint cj = hand.GetComponent<ConfigurableJoint> ();
                cj.anchor = Vector3.zero;
                

            }
            if (Input.GetKey (KeyCode.V)) {

                xoff += Input.GetAxis ("Mouse X") * sens;
                if (xoff > 1)
                    xoff = 1;
                else if (xoff < -1)
                    xoff = -1;

                yoff += Input.GetAxis ("Mouse Y") * sens;
                if (yoff > 1)
                    yoff = 1;
                else if (yoff < -1)
                    yoff = -1;

                

            } else if (Input.GetKeyUp (KeyCode.V)) {

                // hand.transform.Rotate (-90, 0, 0);
                // hand.transform.localPosition = handRestLocalPos;
                swingTime = swingDur;
                v = hand.transform.localPosition;
                v.Set (v.x, v.y, cam.transform.position.z);
                swingAxis = cam.transform.localToWorldMatrix.MultiplyVector (Vector3.Cross (startPos, v).normalized);
                Look.enabled = true;
            }
        } else {
            swingTime -= Time.deltaTime;
            hand.transform.RotateAround (cam.transform.position, swingAxis, -(100 / swingDur) * Time.deltaTime);

            if (swingTime <= 0) {
                swingTime = 0;
                hand.transform.rotation = new Quaternion ();
                hand.transform.SetParent (oldParent);
                hand.transform.localPosition = oldLocalPos;

                xoff = 0;
                yoff = 0;
            }

        }
    }

    // Update is called once per frame
    // void UpdateOld () {
    //     if (swingTime == 0) {
    //         if (Input.GetKeyDown (KeyCode.V)) {

    //             // place the hand one unit away from the camera
    //             startPos = Vector3.forward;
    //             // startPos = cam.transform.localRotation*startPos + cam.transform.localPosition;
    //             // startPos = cam.transform.localPosition;
    //             // startPos.Set (startPos.x, startPos.y, startPos.z + 1);
    //             hand.transform.SetParent (cam.transform);
    //             hand.transform.localPosition = startPos;
    //             Look.enabled = false;
    //             hand.transform.Rotate (90, 0, 0);

    //         }
    //         if (Input.GetKey (KeyCode.V)) {

    //             xoff += Input.GetAxis ("Mouse X") * sens;
    //             if (xoff > 1)
    //                 xoff = 1;
    //             else if (xoff < -1)
    //                 xoff = -1;

    //             yoff += Input.GetAxis ("Mouse Y") * sens;
    //             if (yoff > 1)
    //                 yoff = 1;
    //             else if (yoff < -1)
    //                 yoff = -1;

    //             v = startPos;
    //             v.Set (v.x + xoff, v.y + yoff, v.z);
    //             hand.transform.localPosition = v;

    //         } else if (Input.GetKeyUp (KeyCode.V)) {

    //             // hand.transform.Rotate (-90, 0, 0);
    //             // hand.transform.localPosition = handRestLocalPos;
    //             swingTime = swingDur;
    //             v = hand.transform.localPosition;
    //             v.Set (v.x, v.y, cam.transform.position.z);
    //             swingAxis = cam.transform.localToWorldMatrix.MultiplyVector (Vector3.Cross (startPos, v).normalized);
    //             Look.enabled = true;
    //         }
    //     } else {
    //         swingTime -= Time.deltaTime;
    //         hand.transform.RotateAround (cam.transform.position, swingAxis, -(100 / swingDur) * Time.deltaTime);

    //         if (swingTime <= 0) {
    //             swingTime = 0;
    //             hand.transform.rotation = new Quaternion ();
    //             hand.transform.SetParent (prevParent);
    //             hand.transform.localPosition = handRestLocalPos;

    //             xoff = 0;
    //             yoff = 0;
    //         }

    //     }
    // }
}