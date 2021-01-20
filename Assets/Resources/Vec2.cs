using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vec2{

    public int x, y;
    public Vec2(int xi, int yi){
        x = xi;
        y = yi;
    }

    public static Vector2 quickRot90(Vector2 v){
        Vector2 v2 = new Vector2(v.y, v.x*(-1));
        return v2;
    }

    public static Vector2 quickRot90(float x, float y){
        return new Vector2(y, x*(-1));
        
    }

    public static Vector2 quickRotN90(float x, float y){
        return new Vector2(y*(-1), x);
        
    }

}