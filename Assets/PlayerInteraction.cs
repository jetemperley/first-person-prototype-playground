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
            Debug.Log("pressed E");
            ray = new Ray (cam.transform.position, cam.transform.forward);
            if (Physics.Raycast (ray, out hit) && hit.distance < 4) {
                Debug.Log($"Raycast hit {hit.collider.gameObject.name}");
                use (hit.collider.gameObject);

            }
        }
        
    }

    void use (GameObject g) {
       
        Debug.Log("using item");
        InvObj io = g.GetComponent<InvObj> ();
        Usable use = g.GetComponent<Usable> ();
        Debug.Log($"io null {io == null}");
        if (io != null) {
            Debug.Log("adding to hands");
            inv.addToHandsL (g);
        } else if (use != null){
             // Debug.Log("using " + g.name);
            use.use();
        }
    }

    
}