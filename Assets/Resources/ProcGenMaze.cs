using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProcGenMaze : MonoBehaviour {
    // Start is called before the first frame update

    void Start () {
        int size = 39, rubbleChance = 10;


        Maze m = new Maze (size, size);
        // GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // plane.transform.position = new Vector3(size*5 - 5 - (size*10)/2, 0, size*5 -5 -(size*10)/2);
        // plane.transform.localScale= new Vector3(size, 1, size);

        // Instantiate (plane);
        int[][] data = m.grid;
        GameObject maze = new GameObject();
        GameObject block = (GameObject)Resources.Load ("x10Cube");
        GameObject floor = (GameObject)Resources.Load ("x10Floor");
        GameObject rub = (GameObject)Resources.Load ("BigRubble");
        GameObject doors = (GameObject)Resources.Load ("GladeDoors");
        Vector3 pos = new Vector3 ();
        Vector3 altPos = new Vector3 ();
        Quaternion rot = new Quaternion ();

        for (int z = 0; z < data.Length; z++) {
            for (int x = 0; x < data[z].Length; x++) {
                pos.Set (x * 10 - (size*10)/2, 0, z * 10 - (size*10)/2);
                if (data[z][x] == 0) {
                    GameObject obj = (GameObject)Instantiate(block, pos, rot);
                    addObjToParent(obj, maze);
                } else if (data[z][x] == 1) {
                    GameObject obj = (GameObject)Instantiate (floor, pos, rot);
                    addObjToParent(obj, maze);
                    int rand = Maze.rand.Next(0, 100);
                    if (rand < rubbleChance){
                        GameObject obj2 = (GameObject)Instantiate(rub, pos, rot);
                        addObjToParent(obj2, maze);
                    }
                } else if (data[z][x] == 2){
                    GameObject obj = (GameObject)Instantiate (floor, pos, rot);
                    addObjToParent(obj, maze);
                    GameObject obj2 = (GameObject)Instantiate (doors, pos, rot);
                    addObjToParent(obj2, maze);
                }

            }
        }
        saveObject(maze);

        Debug.Log("finished gen maze");
    }

    void addObjToParent(GameObject child, GameObject parent){
        child.transform.parent = parent.transform;
    }

    void saveObject(GameObject obj){
        PrefabUtility.SaveAsPrefabAsset(obj, "Assets/Resources/maze.prefab");
    }

    // Update is called once per frame
    void Update () {

    }
}