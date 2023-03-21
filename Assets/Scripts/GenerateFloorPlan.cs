using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloorPlan : MonoBehaviour
{
    // Size constraints

    private float w; // house width
    private float l; // house length
    private float h = 3.0f;
    private float thickness = 0.2f;
    private float maxLength = 20.0f;
    private float minLength = 10.0f;   
    private float seg = 3.0f;
    List<GameObject> instantiated = new List<GameObject>();
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        w = Random.Range(minLength, maxLength);
        l = Random.Range(minLength, maxLength);
        //make floor
        GameObject floor = Instantiate(cube, new Vector3(w/2, 0, l/2), Quaternion.identity);
        floor.transform.localScale = new Vector3(w, thickness, l);
        //instantiated.Add(floor); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            if (instantiated.Count > 0) {
                foreach (GameObject o in instantiated) {
                    Destroy(o);
                }
                instantiated.Clear();
            }
            var start = Random.Range(seg, 2 * seg);
            goDown(new Vector3(0, 0, start));
        }
    }

    // pick random l, go down random dist, go left or right random dist
    void goDown(Vector3 offset) { // x offset
        var d = Random.Range(seg, 2 * seg);

        var p = offset + new Vector3(d/2, 0, 0);
        GameObject wall = Instantiate(cube, p, Quaternion.identity);
        wall.transform.localScale = new Vector3(d, h, thickness);
        instantiated.Add(wall);
        p.x = offset.x + d;     

        if (hitBounds(p)) {
            return;
        } else {
            // pick a direction
            var dir = Random.Range(0, 3);
            if (dir == 0) {
                goRight(p);
            } else if (dir == 2){
                goLeft(p);
            } else {
                goRight(p);
                goLeft(p);
            }
        }
    }

    void goUp(Vector3 offset) { // go in x dir
        var d = Random.Range(seg, 2 * seg);

        var p = offset - new Vector3(d/2, 0, 0);
        GameObject wall = Instantiate(cube, p, Quaternion.identity);
        wall.transform.localScale = new Vector3(d, h, thickness);
        instantiated.Add(wall);
        p.x = offset.x - d;     

        if (hitBounds(p)) {
            return;
        } else {
            // pick a direction
            var dir = Random.Range(0, 3);
            if (dir == 0) {
                goRight(p);
            } else if (dir == 2){
                goLeft(p);
            } else {
                goRight(p);
                goLeft(p);
            }
        }
    }

    void goRight(Vector3 offset) { // go in z dir
        var d = Random.Range(seg, 2 * seg);

        var p = offset + new Vector3(0, 0, d/2);
        GameObject wall = Instantiate(cube, p, Quaternion.identity);
        wall.transform.localScale = new Vector3(thickness, h, d);
        instantiated.Add(wall);
        p.z = offset.z + d;     

        if (hitBounds(p)) {
            return;
        } else {
            var dir = Random.Range(0, 3);
            if (dir == 0) {
                goUp(p);
            } else if (dir == 2){
                goDown(p);
            } else {
                goUp(p);
                goDown(p);
            }
        }
    }

    void goLeft(Vector3 offset) { // go in z dir
        var d = Random.Range(seg, 2 * seg);

        var p = offset - new Vector3(0, 0, d/2);
        GameObject wall = Instantiate(cube, p, Quaternion.identity);
        wall.transform.localScale = new Vector3(thickness, h, d);
        instantiated.Add(wall);
        p.z = offset.z - d;     

        if (hitBounds(p)) {
            return;
        } else {
            var dir = Random.Range(0, 3);
            if (dir == 0) {
                goUp(p);
            } else if (dir == 2){
                goDown(p);
            } else {
                goUp(p);
                goDown(p);
            }
        }
    }

    bool hitBounds(Vector3 p) {
        return (p.x <= 0 || p.z <= 0 || p.x >= w || p.z >= l);
    }
}
