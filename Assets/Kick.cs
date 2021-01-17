using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour {

    float kickTime = 0;
    public float kickDur = 1, kickSpeed = 1, stamCost = 10;
    Vector3 restPos;
    Rigidbody rb;
    MeshRenderer mr;
    Health stats;
    // Start is called before the first frame update
    void Start () {
        restPos = gameObject.transform.localPosition;
        rb = gameObject.GetComponent<Rigidbody> ();
        mr = gameObject.GetComponent<MeshRenderer> ();
        stats = gameObject.transform.parent.gameObject.GetComponent<Health> ();
    }

    // Update is called once per frame
    void FixedUpdate () {
        kickWithThrow ();
    }

    void kickWithKinematic () {

        if (kickTime > 0) {
            float kickDist = kickSpeed * (kickDur - kickTime);
            Vector3 kickDir = new Vector3 (0, 0, kickDist);
            Quaternion rot = Quaternion.Euler (0, transform.parent.localEulerAngles.y, 0);
            kickDir = rot * kickDir;
            Vector3 lp = rot * restPos;
            Vector3 p = gameObject.transform.parent.position;
            rb.MovePosition (p + kickDir + lp);

            if (kickTime <= 0) {
                // reset the position
                transform.localPosition = restPos;
                kickTime = 0;
                disable ();
            }
            // dir = Quaternion.Euler (0, transform.localEulerAngles.y, 0) * dir; USE THIS TO GET KICK DIR
        } else if (Input.GetKeyDown (KeyCode.F) && stats.getStam () >= 10) {
            kickTime = kickDur;
            stats.addStam (-stamCost);
            enable ();
        }
    }

    void kickWithThrow () {

        if (Input.GetKeyDown (KeyCode.F) && stats.getStam () >= 10 && kickTime == 0) {

            kickTime = kickDur;
            stats.addStam (-stamCost);
            enable ();
            Quaternion rot = Quaternion.Euler (0, transform.parent.localEulerAngles.y, 0);
            Vector3 kickDir = new Vector3 (0, 0, kickSpeed);
            rb.velocity = (rot*kickDir) + transform.parent.gameObject.GetComponent<Rigidbody>().velocity;

        } else if (kickTime > 0) {

            kickTime -= Time.fixedDeltaTime;

            if (kickTime <= 0) {

                transform.localPosition = restPos;
                kickTime = 0;
                disable ();
            }
        }
    }

    void disable () {
        gameObject.GetComponent<Collider> ().enabled = false;
        rb.isKinematic = true;
        mr.enabled = false;

    }

    void enable () {
        gameObject.GetComponent<Collider> ().enabled = true;
        rb.isKinematic = false;
        mr.enabled = true;
    }
}