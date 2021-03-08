using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightDetection : MonoBehaviour {
    /*
        Simple class that attaches to a collider and
        acts as an area of vision, when a player collider enters,
        triggers the enemy targeting
    */
    public Enemy enemy;
    Ray ray;
    RaycastHit hit;

    void Start () { }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerStay (Collider c) {

        Vector3 t = c.gameObject.transform.position;
        if (canSee (c.gameObject)) {
            enemy.setTarget (t);
            // Debug.Log("targeting player");
            return;
        }

    }

    public bool canSee (GameObject g) {
        Vector3 direction = (g.transform.position - transform.position).normalized;
        ray = new Ray (transform.position, direction);
        int mask = (1 << 8);
        mask = mask | 1;
        if (Physics.Raycast (ray, out hit, 100f, mask) && hit.collider.gameObject == g) {
            return true;
        }

        if (hit.collider != null) {
            Debug.Log ($"checking if see {g.name}, failed hit, hit {hit.collider.gameObject}");
        }
        return false;
    }
}