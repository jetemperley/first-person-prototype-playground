using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour, Usable
{
    GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void use(){
        Debug.Log("used door lock");
        DoorControls dc = door.GetComponent<DoorControls>();
        dc.unlock();
        dc.open();
    }

}
