using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// represents an individual object that can be accepted by an inventory
public class InvObj : MonoBehaviour 
{
    public int shape = 1;
    public int[,] top;
    public Sprite img;
    // {
    //     get {return top;}
    //     set {top = value;}
    // }
    

    void Start()
    {
        top = InvObj.getTop(shape);

    }

    // Update is called once per frame
    void Update()
    {
        


    }


    static int[,] getTop(int idx){
        switch (idx){
            case 0:
                return new int[0,0];
                break;
            case 1:
                return new int[,] {{1}};
                break;
            case 2:
                return new int[,] {{1, 1}};
                break;
            case 3:
                return new int[,] {{1, 1, 1}};
                break;
            case 4:
                return new int[,] {{1, 1, 1, 1}};
                break;
            case 5:
                return new int[,] {{1, 1, 1, 1, 1}};
                break;
            case 6: // small L
                return new int[,] {{1, 1}, {1, 0}};
                break;
            case 7: // square
                return new int[,] {{1, 1}, {1, 1}};
                break;
            case 8: // half t
                return new int[,] {{1, 1, 1}, {0, 1, 0}};
                break;
            case 9: // full L
                return new int[,] {{1, 1, 1}, {1, 0, 0}};
                break;
            case 10: // cross
                return new int[,] {{1, 0, 0}, {1, 1, 1}, {1, 0, 0}};
                break;
        }
        return null;
    }

}
