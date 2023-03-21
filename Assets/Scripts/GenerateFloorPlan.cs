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
        instantiated.Add(floor); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            generateVerticalWall(0.0f);
        }
    }

    // pick random l, go down random dist, go left or right random dist
    void generateVerticalWall(float x) {
        var start = Random.Range(seg, l-seg);
        var d = Random.Range(seg, w);
        if (d > w-seg){
            d = w;
        }
        GameObject wall = Instantiate(cube, new Vector3(x, 0, start), Quaternion.identity);
        wall.transform.localScale = new Vector3(d, h, thickness);
        instantiated.Add(wall);     
    }
}
