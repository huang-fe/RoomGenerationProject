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

    private Vector3 roomDimension; // x = width, y = height, z = length
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
        roomDimension = new Vector3(Random.Range(minLength, maxLength), wallHeight, Random.Range(minLength, maxLength));
        // change scaling
        walls[0].transform.localScale = new Vector3(wallThickness, roomDimension.y, roomDimension.z);
        walls[1].transform.localScale = new Vector3(wallThickness, roomDimension.y, roomDimension.z);
        walls[2].transform.localScale = new Vector3(roomDimension.x, roomDimension.y, wallThickness);
        walls[3].transform.localScale = new Vector3(roomDimension.x, roomDimension.y, wallThickness);
        walls[4].transform.localScale = new Vector3(roomDimension.x, 1, roomDimension.z); // weird
        // change position
        walls[0].transform.position = new Vector3(roomDimension.x/2, 0, 0);
        walls[1].transform.position = new Vector3(-roomDimension.x/2, 0, 0);
        walls[2].transform.position = new Vector3(0, 0, roomDimension.z/2);
        walls[3].transform.position = new Vector3(0, 0, -roomDimension.z/2);
    }

    void generateFloorItems() {
        float type = Random.Range(0.0f, 1.0f);
        var pos = getRandomPos(-roomDimension.y/2, -roomDimension.z/2, roomDimension.z/2, -roomDimension.x/2, -roomDimension.x/2);
        if (type < 0.1f) { // 10%
            generateSmallItem(pos);
        } else if (type < 0.4f) { // 30%
            generateStackable(pos);
        }
        else { // 60%
            generateFurniture(pos);
        } 
    }

    void generateFurniture(var pos) {
        Instantiate(getRandomItem(listFurniture), pos, Quaternion.identity); 
    }

    // generates one stackable item & potentially things on top
    void generateStackable(var pos, int layer) { 
        GameObject obj = getRandomItem(listStackable);
        Vector3 itemHeight = new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
        Instantiate(obj, pos + itemHeight, Quaternion.identity);
        layer--;
        generateStackable(, layer);


        if (layer == 0) { // top layer, small items
            int maxSmallItems = 5; // depend on size of what's below
            int n = Random.Range(0, maxSmallItems);
            for (int i = 0; i < n; i++) {
                var randomPos = getRandomPos();
                instantiateSmallItem(randomPos);
            }
        } else {
            GameObject obj = getRandomItem(listStackable);
            Vector3 itemHeight = new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
            Instantiate(obj, pos + itemHeight, Quaternion.identity);
            layer--;
            generateStackable(, layer);
        }
    }

    // generates one small item
    void generateSmallItem(var pos) { 
        // get random asset
        Instantiate(getRandomItem(listSmall), pos, Quaternion.identity);
    }

    Vector3 getRandomPos(float floor, float l, float r, float t, float d) { // lrtd = -z, z, -x, x
        var position = new Vector3(Random.Range(l, r), floor, Random.Range(d, t));
        // check for collisions

        return position;
    }

    GameObject getRandomItem(List list) {
        int itemIndex = Random.Range(0, list.size()-1);
        return list[itemIndex];
    }
}
