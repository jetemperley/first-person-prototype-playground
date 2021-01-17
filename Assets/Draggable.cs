using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Rigidbody rb;
    Vector3 offset;
    float mz;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        
        mz = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - getMouseAsWorld();

    }

    Vector3 getMouseAsWorld(){
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mz;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }



    void OnMouseDrag(){
        // Vector3 pos = getMouseAsWorld();
        rb.MovePosition(getMouseAsWorld() + offset);
        rb.velocity = new Vector3(0, 0, 0);
    }   
}
