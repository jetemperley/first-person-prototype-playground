using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public Camera cam;
    GameObject hand, handItem;
    Vector3 startPos, oldLocalPos, oldAnchor;
    Quaternion handLocalRot, startRot;
    InvPlayer inv;
    float swingTime = 0, swingDur = 0.5f, yrot = 0, aLimit = 50, xrot = 0, springStrength = 0;
    float sens = 1;
    Vector3 v;
    float xoff = 0, yoff = 0;
    Vector3 swingAxis;
    Transform oldParent;
    Rigidbody rb;
    ConfigurableJoint cj;
    ConfigurableJointMotion[] oldLocks;
    SoftJointLimit[] oldLimits;
    float range = 20;


    void Start () {
        startPos = new Vector3 (0, 0, 1);
        v = new Vector3 ();
        inv = gameObject.GetComponent<InvPlayer> ();

        hand = inv.handsAnchorR;
        oldLocalPos = hand.transform.localPosition;
        oldParent = hand.transform.parent;
        cj = hand.GetComponent<ConfigurableJoint> ();
        oldAnchor = cj.anchor;
        rb = hand.GetComponent<Rigidbody> ();
        springStrength = cj.angularXDrive.positionSpring;

        oldLocks = new ConfigurableJointMotion[6];
        oldLocks[0] = cj.xMotion;
        oldLocks[1] = cj.yMotion;
        oldLocks[2] = cj.zMotion;
        oldLocks[3] = cj.angularXMotion;
        oldLocks[4] = cj.angularYMotion;
        oldLocks[5] = cj.angularZMotion;

        oldLimits = new SoftJointLimit[4];
        oldLimits[0] = cj.lowAngularXLimit;
        oldLimits[1] = cj.highAngularXLimit;
        oldLimits[2] = cj.angularYLimit;
        oldLimits[3] = cj.angularZLimit;
    }

    void resetOldLocks () {
        cj.xMotion = oldLocks[0];
        cj.yMotion = oldLocks[1];
        cj.zMotion = oldLocks[2];
        cj.angularXMotion = oldLocks[3];
        cj.angularYMotion = oldLocks[4];
        cj.angularZMotion = oldLocks[5];
    }

    void resetOldLimits () {

        cj.lowAngularXLimit = oldLimits[0];
        cj.highAngularXLimit = oldLimits[1];
        cj.angularYLimit = oldLimits[2];
        cj.angularZLimit = oldLimits[3];

    }

    void Update () {
        if (swingTime == 0) {
            if (Input.GetKeyDown (KeyCode.V)) {
                handItem = inv.handsItemR;
                if (handItem == null)
                    return;
                
                DestroyImmediate(handItem.GetComponent<FixedJoint>());

                // place the hand one unit away from the camera
                handItem.transform.localRotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);

                handItem.transform.position = cam.transform.position + cam.transform.right + cam.transform.forward;

                
                swingTime = swingDur;
                // initiate swing, swing towards te ceentre of the screen
                Vector3 sforce = -cam.transform.right*10;
                //sforce = sforce.normalized*1000; 
                handItem.GetComponent<Rigidbody>().AddForce (sforce, ForceMode.VelocityChange);

            } else if (Input.GetKey (KeyCode.V)) {
                
                float camx = -cam.transform.localEulerAngles.x;
                // convert between the %360 euler angle (always positive)
                // and a range that can go negative for the joint

                // alter the x rotations so the hand can go below the starting limits:


                // rotate the arm around the z axis based on its position relative to the camera forward (global)
                Vector3 v = hand.transform.localPosition;
                v.z = 0;

                // handItem.GetComponent<Rigidbody>().AddRelativeForce (force);

            } else if (Input.GetKeyUp (KeyCode.V)) {
                
            }

        } else {
            swingTime -= Time.deltaTime;

            if (swingTime <= 0) {
                swingTime = 0;

                handItem.transform.position = hand.transform.position;
                handItem.transform.localRotation = Quaternion.identity;
                FixedJoint fj = handItem.AddComponent<FixedJoint>();
                fj.connectedBody = gameObject.GetComponent<Rigidbody>();
                
            }

        }
    }

    void rotateArm (float x, float y, float z) {
        Quaternion q = Quaternion.Euler (x, y, z);
        rb.MoveRotation (rb.rotation * q);
        cj.targetRotation = cj.targetRotation * q;
    }

    void transformRotArm (float x, float y, float z) {
        Quaternion q = Quaternion.Euler (x, y, z);
        hand.transform.localRotation = hand.transform.localRotation * q;

    }

    void rotateArm (Quaternion q) {
        hand.transform.localRotation = hand.transform.localRotation * q;
        cj.targetRotation = cj.targetRotation * q;
    }

    void rotateArmXTo (float x) {
        Vector3 v = cj.targetRotation.eulerAngles;
        cj.targetRotation = Quaternion.Euler (x, v.y, v.z);
        v = hand.transform.localEulerAngles;
        hand.transform.localRotation = Quaternion.Euler (x, v.y, v.z);
    }

    void Update2 () {
        if (swingTime == 0) {
            if (Input.GetKeyDown (KeyCode.V)) {

                // place the hand one unit away from the camera
                hand.transform.SetParent (cam.transform);
                hand.transform.localPosition = startPos;
                hand.transform.localRotation = Quaternion.identity;
                cj.anchor = Vector3.back;
                startRot = cam.transform.localRotation;
                startRot.x = -startRot.x;
                cj.targetRotation = startRot;
                // cj.targetRotation = Quaternion.identity;
                Look.enabled = false;

            }
            if (Input.GetKey (KeyCode.V)) {

                float x = Input.GetAxis ("Mouse X") * sens;
                xoff += x;
                if (xoff > aLimit)
                    xoff = aLimit;
                else if (xoff < -aLimit)
                    xoff = -aLimit;

                float y = Input.GetAxis ("Mouse Y") * sens;
                yoff += y;
                if (yoff > aLimit)
                    yoff = aLimit;
                else if (yoff < -aLimit)
                    yoff = -aLimit;

                Quaternion q = startRot * Quaternion.Euler (yoff, -xoff, 0);
                // Quaternion q = Quaternion.LookRotation(j, Vector3.up);
                cj.targetRotation = q;

            } else if (Input.GetKeyUp (KeyCode.V)) {

                swingTime = swingDur;
                cj.targetRotation = startRot;
                Look.enabled = true;
            }
        } else {
            swingTime -= Time.deltaTime;
            hand.transform.RotateAround (cam.transform.position, swingAxis, -(100 / swingDur) * Time.deltaTime);

            if (swingTime <= 0) {
                swingTime = 0;
                hand.transform.SetParent (oldParent);
                hand.transform.localPosition = oldLocalPos;
                hand.transform.localRotation = Quaternion.identity;
                cj.targetRotation = Quaternion.identity;
                cj.anchor = oldAnchor;

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