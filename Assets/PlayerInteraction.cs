using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    InvPlayer inv;
    RaycastHit hit;
    Ray ray;
    public Camera cam;

    void Start () {
        inv = gameObject.GetComponent<InvPlayer> ();
    }

    // Update is called once per frame
    void Update () {
        // Debug.Log("" + Input.mousePosition.z);
        if (Input.GetKeyDown (KeyCode.E)) {
            ray = new Ray (cam.transform.position, cam.transform.forward);
            if (Physics.Raycast (ray, out hit) && hit.distance < 4) {
                use (hit.collider.gameObject);

            }
        }
        
    }

    void use (GameObject g) {
       
        InvObj io = g.GetComponent<InvObj> ();
        Usable use = g.GetComponent<Usable> ();
        if (io != null) {
            inv.addToHands (io.gameObject);
        } else if (use != null){
             // Debug.Log("using " + g.name);
            use.use();
        }
    }
}