using UnityEngine;

public class Move : MonoBehaviour {

    float xd = 0, yd = 0, space = 0, accelFactor = 0;
    public Rigidbody rb;
    bool feetOnGroundFlag = true;
    public float accel = 0.15f, grabRatio = 0.5f, maxSpeed = 5f,
        speedIncFac = 1.0f, speedDecFac = 1.0f, slowRate = 8f;
    Vector3 dir, temp, temp2;
    float moveN = 0;

    void Start () {
        dir = new Vector3 ();
        temp = new Vector3 ();
        temp2 = new Vector3 ();
    }

    void FixedUpdate () {
        moveWithPosition ();
        if (feetOnGroundFlag 
        && Input.GetAxisRaw ("Horizontal") == 0 
        && Input.GetAxisRaw ("Vertical") == 0){
            // Debug.Log("slowing");
            slow();
        }
        feetOnGroundFlag = false;

    }

    void slow(){
        float slowRateFac = slowRate*Time.fixedDeltaTime;
        Vector3 horVel = getHorVel();
        if (horVel.magnitude - slowRateFac < 0) {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        } else {
            Vector3 nv = horVel.normalized*(horVel.magnitude-slowRateFac);
            nv.Set(nv.x, rb.velocity.y, nv.z);
            rb.velocity = nv;
        }

    }

    void moveWithVel () {

        accelFactor = accel * Time.fixedDeltaTime * 100;

        float maxCtxSpeed = 0;
        if (feetOnGroundFlag){
            maxCtxSpeed = maxSpeed;
            accelFactor = accel * Time.fixedDeltaTime*100;
        } else {
            maxCtxSpeed = maxSpeed/1.75f;
            accelFactor = accel * Time.fixedDeltaTime*25;
        }

        xd = Input.GetAxisRaw ("Horizontal");
        yd = Input.GetAxisRaw ("Vertical");
        space = Input.GetAxisRaw ("Jump");

        // dir is the direction the input wants 
        dir.Set (xd, 0, yd);
        dir = dir.normalized * accelFactor;
        dir = Quaternion.Euler (0, transform.localEulerAngles.y, 0) * dir;

        Vector3 curHorVel = new Vector3 (rb.velocity.x, 0, rb.velocity.z);

        if (curHorVel.magnitude > maxCtxSpeed) {
            if ((curHorVel + dir).magnitude > curHorVel.magnitude) {
                // rb.velocity = (rb.velocity + dir).normalized * rb.velocity.magnitude;
                curHorVel = (curHorVel + dir).normalized*curHorVel.magnitude;
                curHorVel.Set(curHorVel.x, rb.velocity.y, curHorVel.z);
                rb.velocity = curHorVel;
            } else {
                rb.velocity += dir;
            }
        } else if ((curHorVel + dir).magnitude > maxCtxSpeed) {
            curHorVel = (curHorVel + dir).normalized * maxCtxSpeed;
            curHorVel.Set (curHorVel.x, rb.velocity.y, curHorVel.z);
            rb.velocity = curHorVel;
        } else {
            rb.velocity += dir;
        }

    }

    void moveWithForce () {

        accelFactor = accel * Time.fixedDeltaTime * 100;

        xd = Input.GetAxisRaw ("Horizontal");
        yd = Input.GetAxisRaw ("Vertical");
        space = Input.GetAxisRaw ("Jump");

        if (xd != 0 || yd != 0) {
            moveN += speedIncFac;
        } else if (moveN > 0) {
            moveN -= speedDecFac;
        } else if (moveN < 0) {
            moveN = 0;
        }

        // dir is the direction the input wants 
        dir.Set (xd, 0, yd);
        dir = dir.normalized * accelFactor;
        dir = Quaternion.Euler (0, transform.localEulerAngles.y, 0) * dir;

        if (feetOnGroundFlag) {
            // Debug.Log("" + (dir/moveN).magnitude);
            if (moveN > 0)
                dir = dir / moveN;
            rb.AddForce (dir);
        }

    }

    void moveWithPosition(){
        xd = Input.GetAxis ("Horizontal");
        yd = Input.GetAxis ("Vertical");
        space = Input.GetAxisRaw ("Jump");

        dir.Set (xd, 0, yd);
        dir = dir * maxSpeed * Time.fixedDeltaTime;
        dir = Quaternion.Euler (0, transform.localEulerAngles.y, 0) * dir;

        rb.MovePosition(transform.position + dir);

    }

    

    // void simpleMove () {
    //     accelFactor = accel * Time.fixedDeltaTime;

    //     xd = Input.GetAxis ("Horizontal") * accelFactor;
    //     yd = Input.GetAxis ("Vertical") * accelFactor;
    //     space = Input.GetAxisRaw ("Jump");

    //     dir.Set (xd, 0, yd);
    //     dir = Quaternion.Euler (0, transform.localEulerAngles.y, 0) * dir;

    //     // temp = the volocity if we added dir
    //     // temp.Set (rb.velocity.x/Time.fixedDeltaTime + dir.x, 0, 
    //     //     rb.velocity.z/Time.fixedDeltaTime + dir.z);

    //     // // temp2 = the current velocity
    //     // temp2.Set (rb.velocity.x/Time.fixedDeltaTime, 0, 
    //     //     rb.velocity.z/Time.fixedDeltaTime);

    //     // if (temp.magnitude > maxSpeed/Time.fixedDeltaTime && 
    //     //     temp.magnitude > temp2.magnitude) {
    //     //     // add vel + dir and scale to original mag
    //     //     temp = (temp.normalized * temp2.magnitude);
    //     //     temp.Set (temp.x, 0, temp.y);
    //     //     // move here
    //     //     rb.MovePosition (transform.position + temp);
    //     // } else {
    //     //     // move normally
    //     //     rb.MovePosition (transform.position + dir);
    //     // }
    //     rb.MovePosition (transform.position + dir);

    // }


    Vector3 getHorVel(){
        return new Vector3 (rb.velocity.x, 0, rb.velocity.z);

    }


    public void feetOnGround(){
        feetOnGroundFlag = true;
    }
}