using UnityEngine;

public class Launch : MonoBehaviour {

    bool canLaunch = false, footTrigger = false;
    public Rigidbody rb;
    public float launchForce = 100f;
    public int cooldown = 0, cooldownTime = 50;
    float closestDist = -1f;
    Vector3 closestObj;
    float jumpCounter = 1;

    void Start () {
        // Debug.Log("started launch");
    }

    void FixedUpdate () {

        if (cooldown > 0)
            cooldown--;
        // Debug.Log("can launch " + canLaunch);
        if (Input.GetAxis ("Jump") != 0f && canLaunch) {

            if (footTrigger) {

                rb.AddForce (0, launchForce, 0);
                Debug.Log("strait jump");

            } else {

                closestObj.y = closestObj.y - 0.8f;
                Vector3 ld = transform.position - closestObj;
                // Debug.Log ("co: " + closestObj.x + " " + closestObj.y + " " + closestObj.z + " ");
                ld = ld.normalized * launchForce;
                // Debug.Log ("ld: " + ld.x + " " + ld.y + " " + ld.z + " ");
                rb.AddForce (ld.x / jumpCounter, ld.y / jumpCounter, ld.z / jumpCounter);
                // jumpCounter+= 0.2f;

                Debug.Log ("side jump");
            }

            cooldown = cooldownTime;
            canLaunch = false;
        }
        closestDist = -1f;
        footTrigger = false;

    }

    public void enableLaunch (Collider obj) {
        //Debug.Log("enabling");
        if (cooldown == 0 && Input.GetAxis ("Jump") == 0f) {
            canLaunch = true;
        }

        if (obj != null) {

            if (!footTrigger) {
                Vector3 point = obj.ClosestPoint (transform.position);
                // Debug.Log (obj.gameObject.name);
                float d = Vector3.Distance (transform.position, point);
                // Debug.Log("d = " + d);
                if (closestDist == -1 || d < closestDist) {
                    closestObj = point;
                    closestDist = d;
                }
            }

            // Debug.Log ("closest x =" + closestObj.x + ", + y =" + closestObj.y + ", + z =" + closestObj.z);

        } else {
            Debug.Log ("null obj in trigger stay");
            // closestDist = -1f;
        }
    }

    public void feetOnGround () {
        //Debug.Log("feetOnGround called");
        footTrigger = true;
        jumpCounter = 1;
        //Debug.Log("counter reset");

    }
}