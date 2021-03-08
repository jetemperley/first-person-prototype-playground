using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    Vector3 target; // world space location of target
    Random rand;
    public float speed = 0.2f;
    Rigidbody rb;
    GameObject lat; // obj to hold WS loc transform to look at
    float timer = 0;
    List<GameObject> flock;


    void Start () {
        rand = new Random ();
        target = getRandTarget ();
        rb = gameObject.GetComponent<Rigidbody> ();
        lat = new GameObject ();
        // GameObject.CreatePrimitive(PrimitiveType.Sphere);
        setTarget (target);
        flock = new List<GameObject>();
        // lat.SetActive(true);
    }

    void Update () {

    }

    void FixedUpdate () {
        float speedFac = speed * Time.fixedDeltaTime;

        // if (Vector3.Distance)
        // {
        //     if (//player is in the look angle)
        //     {
        //         // target is player
        //     }
        if ((gameObject.transform.position - target).magnitude < 0.1f || timer > 60) {
            setTarget (transform.position + getRandTarget ());
            Debug.Log ("set random target");
        }
        moveToTarget (target, speedFac);
        timer += Time.fixedDeltaTime;
        updateFlock();
    }

    void moveToTarget (Vector3 tgt, float dist) {

        // Debug.Log("moving");

        tgt = tgt - gameObject.transform.position;

        if (tgt.magnitude > dist) {

            // rb.MovePosition (transform.position + (tgt.normalized * dist));
            // Vector3 v = tgt.normalized;
            // rb.AddForce(v.x, v.y, v.z);
            rb.MovePosition(transform.position + (tgt.normalized * dist));
            
        } else {
            // rb.MovePosition (transform.position + tgt);
            rb.MovePosition(transform.position + (tgt.normalized * dist));
            
        }

    }

    void lookAt (Vector3 dir) {
        lat.transform.position = dir;
        transform.LookAt (lat.transform);
    }

    Vector3 getRandTarget () {

        Vector2 dir = Random.insideUnitCircle * 30;
        // Debug.Log("gend rand " + dir.x + " " + dir.y);
        return new Vector3 (dir.x, 1, dir.y);

    }

    public void setTarget (Vector3 t) {
        target = new Vector3 (t.x, gameObject.transform.position.y, t.z);
        lookAt (target);
        timer = 0;
    }

    void OnCollisionEnter(Collision c){
        if (c.collider.gameObject.CompareTag("Enemy")){
            GameObject g = c.collider.gameObject.transform.parent.gameObject;
            if (!flock.Contains(g)){
                flock.Add(g);
            }
        }
    }

    void updateFlock(){
        for (int i = 0; i < flock.Count; i++){
            GameObject g = flock[i];
            Vector3 v = g.transform.position -transform.position;
            float d = v.sqrMagnitude;
            if (d > 20){
                flock.RemoveAt(i);
                i--;
            } else if (d < 10){
                rb.AddForce(-v.normalized*(20/d));
            }

        }
    }
}