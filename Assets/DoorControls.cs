using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControls : MonoBehaviour, Usable {
    Transform doorForm;
    bool closed = true, broken = false, shouldFix = false;
    public bool locked = true;
    Vector3 rots;
    HingeJoint hj;
    float openPos, closedPos;
    JointSpring s;
    float doorTime, doorDur = 0.2f;

    void Start () {
        doorForm = gameObject.transform;
        hj = gameObject.GetComponent<HingeJoint> ();
        openPos = hj.limits.min;
        closedPos = hj.limits.max;
        s = hj.spring;
        hj.useSpring = false;
    }

    // Update is called once per frame
    void Update () {
        rots = doorForm.localEulerAngles;
        if (doorTime > 0) {
            doorTime -= Time.deltaTime;
            if (doorTime <= 0){
                doorTime = 0;
                hj.useSpring = false;
            }
        } else {
            if (rots.y > 2)
                shouldFix = true;
            if (shouldFix && !broken && rots.y > -2 && rots.y < 2) {
                doorForm.localRotation = Quaternion.Euler (rots.x, 0, rots.z);
                closed = true;
                shouldFix = false;
                FixedJoint fj = gameObject.AddComponent<FixedJoint> ();
                Debug.Log ("added a new fixed joint");
            }
        }
    }

    void OnJointBreak (float force) {
        broken = true;
        closed = false;
        locked = false;
        // Destroy(this, 0.02f);
    }

    public void open () {
        doorTime = doorDur;
        Debug.Log ("opening");
        hj.useSpring = true;
        s.targetPosition = openPos;
        hj.spring = s;
        closed = false;
        shouldFix = false;

        FixedJoint fj = gameObject.GetComponent<FixedJoint> ();
        if (fj != null)
            Destroy (fj, 0.02f);
    }

    public void close () {
        doorTime = doorDur;
        hj.useSpring = true;
        s.targetPosition = closedPos;
        hj.spring = s;
        closed = true;

    }

    public void use () {
        if (!locked) {
            if (closed) {
                open ();
            } else {
                close ();
            }
        }
    }

    public void unlock () {
        locked = false;

    }
}