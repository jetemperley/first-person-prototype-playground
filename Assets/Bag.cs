using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag {

    public GameObject [, ] bag;
    public List<ObjLoc> contents;
    public GameObject UIDisplay; 

    public Bag (int x, int y) {
        bag = new GameObject[x, y];
        contents = new List<ObjLoc> ();
    }

    // adds a gameObject with a InvObject component to
    // the bag with the upper left object.top at index x, y
    public bool addItem (GameObject g, int x, int y) {
        int[, ] obj = g.GetComponent<InvObj> ().top;
        if (canFit (g, x, y)) {
            for (int xi = 0; xi < obj.GetLength (0); xi++) {
                for (int yi = 0; yi < obj.GetLength (1); yi++) {
                    if (obj[xi, yi] == 1) {
                        bag[xi + x, yi + y] = g;
                    }
                }
            }
            contents.Add (new ObjLoc (x, y, g));
            // alter the item so it doesnt just fal onto the ground
            g.SetActive(false);
            return true;
        }
        return false;

    }

    // DO NOT USE
    // public bool canFit (int[, ] obj, int x, int y) {

    //     for (int xi = 0; xi < obj.GetLength (0); xi++) {
    //         for (int yi = 0; yi < obj.GetLength (1); yi++) {
    //             if (obj[xi, yi] == 1 && (!isBagRange (x + xi, y + yi) || bag[x + xi, y + yi] != null)) {

    //                 return false;

    //             }
    //         }
    //     }
    //     return true;
    // }

    public bool canFit (GameObject g, int x, int y) {
        if (g == null)
            return false;
        int[,] obj = g.GetComponent<InvObj>().top;
        for (int xi = 0; xi < obj.GetLength (0); xi++) {
            for (int yi = 0; yi < obj.GetLength (1); yi++) {
                if (obj[xi, yi] == 1 && (!isBagRange (x + xi, y + yi) || (bag[x + xi, y + yi] != null && (bag[x + xi, y + yi] != g)))) {

                    return false;

                }
            }
        }
        
        return true;
    }

    bool isBagRange (int x, int y) {
        if (x >= 0 && x < bag.GetLength (0) && y >= 0 && y < bag.GetLength (1))
            return true;
        return false;
    }

    // removes the object at the index x, y
    // aswell as all of its occupancies based on
    // all of the same references in bag 
    public GameObject removeItem (int x, int y) {
        GameObject g = null;
        if (isBagRange (x, y) && bag[x, y] != null) {
            g = bag[x, y];
            for (int xi = 0; xi < bag.GetLength (0); xi++) {
                for (int yi = 0; yi < bag.GetLength (1); yi++) {
                    if (bag[xi, yi] == g)
                        bag[xi, yi] = null;
                }
            }
            for (int i = 0; i < contents.Count; i++) {
                if (contents[i].g == g) {
                    contents.RemoveAt (i);
                    break;
                }
            }
            g.SetActive(true);
            // reset position?

            
        }
        return g;
    }

    GameObject get(int x, int y){
        if (isBagRange(x, y)){
            return bag[x, y];
        }
        return null;
    }
}