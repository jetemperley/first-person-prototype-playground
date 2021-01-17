using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour {
    /*
        Simple class that attaches to a collider and
        acts as an area of vision, when a player collider enters,
        triggers the enemy targeting
    */
    public Enemy enemy;
    // Start is called before the first frame update
    void Start () { }

    // Update is called once per frame
    void Update () {

    }

    void OnTriggerStay (Collider c) {

        Vector3 t = c.gameObject.transform.position;
        if (enemy.canSee (c.gameObject)){
            enemy.setTarget (t);
            // Debug.Log("targeting player");
            return;
        }

    }
}