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

    // Arrays of assets
    public static GameObject[] listFurniture;
    public static GameObject[] listStackable;  
    public static GameObject[] listSmallItems; 

    // Start is called before the first frame update
    void Start()
    {
        walls[4].transform.position = new Vector3(0, -wallHeight/2, 0); // floor

        // load assets into arrays
        listFurniture = Resources.LoadAll<GameObject>("Furniture");
        listSmallItems = Resources.LoadAll<GameObject>("Stackable");
        listSmallItems = Resources.LoadAll<GameObject>("Small Items");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            generateRoom();
            //generateFurniture();
            
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

    // generates one furniture and potentially things on top
    void generateFurniture(Vector3 pos) {
        // 1 furniture
        GameObject obj = getRandomItem(listFurniture);
        float top_of_furniture = pos.y + obj.GetComponent<Collider>().bounds.size.y;
        float obj_width_halved = obj.GetComponent<Collider>().bounds.size.x/2;
        float obj_length_halved = obj.GetComponent<Collider>().bounds.size.z/2;
        Instantiate(obj, pos, Quaternion.identity); 

        // num things on top
        int n_on_top = Random.Range(0, 5);
        for (int i = 0; i < n_on_top; i ++) {
            // new random position
            var newPos = getRandomPos(top_of_furniture, pos.x - obj_width_halved, pos.x + obj_width_halved, pos.z - obj_length_halved, pos.z + obj_length_halved);
            // stackable or on top
            if (Random.Range(0.0f, 1.0f) < 0.5f) { // 50% 
                generateSmallItem(pos);
            } else {
                generateStackable(pos);
            }
        }
    }

    // generates >=1 stackable items & potentially things on top
    void generateStackable(Vector3 pos) { 
        GameObject obj = getRandomItem(listStackable);
        Instantiate(obj, pos, Quaternion.identity);
        var top_of_stack = pos + new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
        
        // n stackable items
        int nStack =  Random.Range(0, 5);
        for (int i = 0; i < nStack; i++) {
            obj = getRandomItem(listStackable);
            Instantiate(obj, top_of_stack, Quaternion.identity);
            top_of_stack = top_of_stack + new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
        }

        float obj_width_halved = obj.GetComponent<Collider>().bounds.size.x/2;
        float obj_length_halved = obj.GetComponent<Collider>().bounds.size.z/2;

        // generate small items
        int nSmall = Random.Range(0, 5); // max small items determined by size of what's below?
        for (int i = 0; i < nSmall; i++) {
            var newPos = getRandomPos(top_of_stack.y, pos.x - obj_width_halved, pos.x + obj_width_halved, pos.z - obj_length_halved, pos.z + obj_length_halved);
            generateSmallItem(newPos);
        }
    }

    // generates one small item
    void generateSmallItem(Vector3 pos) {
        Instantiate(getRandomItem(listSmallItems), pos, Quaternion.identity);
    }

    Vector3 getRandomPos(float floor, float l, float r, float t, float d) { // lrtd = -z, z, -x, x
        var position = new Vector3(Random.Range(l, r), floor, Random.Range(d, t));
        // check for collisions

        return position;
    }

    GameObject getRandomItem(GameObject[] list) {
        int itemIndex = Random.Range(0, list.Length-1);
        return list[itemIndex];
    }
}
