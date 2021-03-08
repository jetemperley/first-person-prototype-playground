using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackV2 : MonoBehaviour {

    public Camera cam;
    GameObject hand;
    Vector3 startPos, oldLocalPos, oldAnchor;
    Quaternion handLocalRot, startRot;
    InvPlayer inv;
    float swingTime = 0, swingDur = 1f, yrot = 0, aLimit = 50, xrot=  0, springStrength = 0;
    float sens = 1;
    Vector3 v;
    float xoff = 0, yoff = 0;
    Transform oldParent;
    Rigidbody rb;
    

    void Start () {
        startPos = new Vector3 (0, 0, 1);
        v = new Vector3 ();
        inv = gameObject.GetComponent<InvPlayer> ();
        
        hand = inv.handsAnchorR;
        oldLocalPos = hand.transform.localPosition;
        rb = hand.GetComponent<Rigidbody> ();

    }

    void Update () {
        if (swingTime == 0) {
            if (Input.GetKeyDown (KeyCode.V)) {

                // place the hand in the starting position
                
                hand.transform.localPosition = Vector3.zero;
                hand.transform.rotation = hand.transform.rotation *Quaternion.Euler(90, 0, 0);
                hand.transform.localPosition = new Vector3(0, cam.transform.localPosition.y, 1);
                

            } else if (Input.GetKey (KeyCode.V)) {
                if (yrot < aLimit) {
                    
                    // wind up the arm
                    yrot += aLimit * Time.deltaTime;


                }
                // cj.targetRotation = cj.targetRotation*
            } else if (Input.GetKeyUp (KeyCode.V)) {
                swingTime = swingDur;
                // initiate swing
                // allow free rotation
               
                // add force
                Vector3 forceDir = gameObject.transform.rotation*(new Vector3(0 , 0, 10)); 
                rb.AddForce(forceDir, ForceMode.VelocityChange);
            }

        } else {
            swingTime -= Time.deltaTime;
            // cj.targetRotation = Quaternion.Euler (0, -(2*yrot / swingDur) * Time.deltaTime, 0) * cj.targetRotation;

            if (swingTime <= 0) {

                // return to rest position
                swingTime = 0;
                
                yrot = 0;
                xrot = 0;
               
            }

        }
    }


}