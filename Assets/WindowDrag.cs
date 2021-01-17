using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDrag : MonoBehaviour {
    Rigidbody rb;
    float position = 0;
    float restPos;
    ConfigurableJoint cj;
    bool drag = false;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody> ();
        restPos = transform.localPosition.z;
        position = transform.localPosition.z;
        cj = gameObject.GetComponent<ConfigurableJoint> ();
    }

    // Update is called once per frame
    void Update () {

    }

    void OnMouseUpAsButton () {
        if (!drag) {

        }

        Debug.Log ("mouseClick");
    }

    void OnMouseUp () {
        drag = false;
    }

    void OnMouseDrag () {
        // Debug.Log("dragging");
        // position = transform.localPosition.x;
        // position += Input.GetAxis("Mouse X")/10;
        // Debug.Log($"position before test {position}");

        // if (restPos - position > 1)
        //     position = restPos - 1;
        // else if (restPos - position < 0)
        //     position = restPos;

        // Debug.Log($"position after test {position}");

        // Vector3 pos = transform.localPosition;
        // pos.Set(position, pos.y, pos.z);
        // transform.localPosition = pos;
        drag = true;
        rb.velocity = Vector3.zero;
        float diff = Input.GetAxis ("Mouse X") / 5;
        Vector3 pos = transform.localPosition;
        if (isInRange (pos.z + diff)) {
            Debug.Log("in range");
            pos.Set (pos.x, pos.y, pos.z + diff);
            transform.localPosition = pos;
        }  else {
            Debug.Log("not in range");
        }
    }

    bool isInRange (float pos) {
        if (pos > restPos - 1 && pos < restPos) {
            return true;
        }
        return false;
    }
}