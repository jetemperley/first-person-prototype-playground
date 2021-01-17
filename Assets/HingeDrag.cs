using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDrag : MonoBehaviour {

    HingeJoint hj;
    Rigidbody rb;
    Vector2 mousePos;
    JointSpring spring;

    void Start () {
        hj = gameObject.GetComponent<HingeJoint> ();
        if (hj == null) {
            Destroy (this, 0.02f);
        }
        rb = gameObject.GetComponent<Rigidbody> ();
        spring = hj.spring;
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate () {
        // if (toRot != 0){

        //     Quaternion q = Quaternion.Euler(0, toRot, 0)*rb.rotation;
        //     rb.MoveRotation(q);
        //     // rb.rotation = q;
        //     transform.localRotation = q;
        // }
    }

    void OnMouseDown () {
        rb.velocity = Vector3.zero;
    }

    void OnMouseDrag () {
        rb.velocity = Vector3.zero;
        float diff = -Input.GetAxis ("Mouse X")*10;
        Debug.Log("" + hj.angle);
        float angle = hj.angle + diff;
        // Debug.Log($"angle {angle} hjang {hj.angle}");
        if (isInHingeRange (angle)) {
            // Debug.Log($"diff {diff} angle {angle} mousex {Input.GetAxis ("Mouse X")}");
            rb.MoveRotation (Quaternion.Euler (0, transform.eulerAngles.y+diff, 0));
        } else {

        }
    }

    void OnMouseUpAsButton(){
        
    }

    bool isInHingeRange (float f) {
        // float rot = hj.angle + f;
        // Debug.Log ($"min {hj.limits.min} max {hj.limits.max} rot {rot}");
        if (f >= hj.limits.min && f <= hj.limits.max) {
            return true;
        }
        return false;
    }
}