using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Generates room shape
* Press space to generate room. Press other buttons to generate furniture
* Ceiling-less world
* Origin is in the middle of the room
*/

public class GenerateRoom : MonoBehaviour
{
    // Size constraints
    public float wallHeight = 3.0f;
    public float wallThickness = 0.4f;

    private Vector3 roomDimension; // width, length, height
    private float maxLength = 20.0f;
    private float minLength = 3.0f;

    public GameObject[] walls; // array of walls    

    // Start is called before the first frame update
    void Start()
    {
        walls[4].transform.position = new Vector3(0, -wallHeight/2, 0); // floor
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            generateRoom();
            generateFurniture();
            
            //var position = new Vector3(Random.Range(minWidth, maxWidth), 0, Random.Range(-10.0f, 10.0f));
            //Instantiate(prefab, position, Quaternion.identity);
        }
    }

    //Instantiate walls (cubes) with same thickness, correct rotations, and width/length distance away from origin
    //Each new instantiate, re-randomize width/length for walls and change their positions (keep track of 4 walls)
    void generateRoom() {
        // randomize room sizes
        roomDimension = new Vector3(Random.Range(minLength, maxLength), Random.Range(minLength, maxLength), wallHeight);
        // change scaling
        walls[0].transform.localScale = new Vector3(wallThickness, wallHeight, roomDimension.y);
        walls[1].transform.localScale = new Vector3(wallThickness, wallHeight, roomDimension.y);
        walls[2].transform.localScale = new Vector3(roomDimension.x, wallHeight, wallThickness);
        walls[3].transform.localScale = new Vector3(roomDimension.x, wallHeight, wallThickness);
        walls[4].transform.localScale = new Vector3(roomDimension.x, 1, roomDimension.y); // weird
        // change position
        walls[0].transform.position = new Vector3(roomDimension.x/2, 0, 0);
        walls[1].transform.position = new Vector3(-roomDimension.x/2, 0, 0);
        walls[2].transform.position = new Vector3(0, 0, roomDimension.y/2);
        walls[3].transform.position = new Vector3(0, 0, -roomDimension.y/2);
    }

    void generateFurniture() {

    }

    Vector3 getFurniturePos(float floor, float l, float r, float t, float d) {
        var position = new Vector3(Random.Range(l, r), floor, Random.Range(d, t));
        // check for collisions
    
        return position;
    }
}
