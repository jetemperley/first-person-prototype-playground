using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLoc {
    public int x = 0, y = 0;
    public GameObject g;

    public ObjLoc (int x, int y, GameObject g) {
        this.x = x;
        this.y = y;
        this.g = g;
    }

}