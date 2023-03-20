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
    private Vector4 roomBounds; // xyzw = -x, x, -z, z
    private float maxLength = 20.0f;
    private float minLength = 5.0f;

    public GameObject[] walls; // array of walls    

    // Arrays of assets
    public static GameObject[] listFurniture;
    public static GameObject[] listStackable;  
    public static GameObject[] listSmallItems; 

    // List of Instantiated Objects
    List<GameObject> instantiated = new List<GameObject>();

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
            if (instantiated.Count > 0) {
                foreach (GameObject o in instantiated) {
                    Destroy(o);
                }
                instantiated.Clear();
            }
            generateRoom();
            int n = Random.Range(5, 20); 
            for (int i = 0; i < n; i ++) {
                generateFloorItems();
            }
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
        
        // empty space is smaller
        roomDimension -= new Vector3(wallThickness, 0, wallThickness);
        roomBounds = new Vector4(-roomDimension.x/2+wallThickness/2, roomDimension.x/2+wallThickness/2, -roomDimension.z/2+wallThickness/2, roomDimension.z/2+wallThickness/2);
    }

    void generateFloorItems() {
        float type = Random.Range(0.0f, 1.0f);
        if (type < 0.4f) {             
            // generateSmallItem(pos);
            generateItem(0.0f, roomBounds, listSmallItems);
        // } else if (type < 0.4f) { 
        //     generateStackable(pos);
        }
        else { 
            generateFurniture(-roomDimension.y/2, roomBounds);
        } 
    }

    // generates one furniture and potentially things on top
    void generateFurniture(float floor, Vector4 b) {
        GameObject obj = getRandomItem(listFurniture);
        var c = obj.GetComponent<BoxCollider>();
        Debug.Log("collider x y z : " + c.size.x + c.size.y + c.size.z);
        // update bounds so item does not collide w wall
        float l = c.size.x;
        if (l < c.size.z) l = c.size.z;
        b += new Vector4(l/2, -l/2, l/2, -l/2);
        floor += c.size.y/2;
        var pos = getRandomPos(floor, b);
        instantiated.Add(Instantiate(obj, pos, Quaternion.identity)); 

        float top_of_furniture = pos.y + c.size.y;
        float obj_width_halved = c.size.x/2;
        float obj_length_halved = c.size.z/2;        
        Vector4 bounds = new Vector4(pos.x - obj_width_halved, pos.x + obj_width_halved, pos.z - obj_length_halved, pos.z + obj_length_halved);

        // num things on top
        int n_on_top = Random.Range(0, 5);
        for (int i = 0; i < n_on_top; i ++) {            
            // stackable or on top
            if (Random.Range(0.0f, 1.0f) < 0.5f) { // 50% 
                // generateSmallItem(top_of_furniture, bounds);
                generateItem(top_of_furniture, bounds, listSmallItems);
            }
            // } else {
            //     generateStackable(pos);
            // }
        }
    }

    // generates >=1 stackable items & potentially things on top
    // void generateStackable(Vector3 pos) { 
    //     GameObject obj = getRandomItem(listStackable);
    //     Instantiate(obj, pos, Quaternion.identity);
    //     var top_of_stack = pos + new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
        
    //     // n stackable items
    //     int nStack =  Random.Range(0, 5);
    //     for (int i = 0; i < nStack; i++) {
    //         obj = getRandomItem(listStackable);
    //         Instantiate(obj, top_of_stack, Quaternion.identity);
    //         top_of_stack = top_of_stack + new Vector3(0, obj.GetComponent<Collider>().bounds.size.y, 0);
    //     }

    //     float obj_width_halved = obj.GetComponent<Collider>().bounds.size.x/2;
    //     float obj_length_halved = obj.GetComponent<Collider>().bounds.size.z/2;

    //     // generate small items
    //     int nSmall = Random.Range(0, 5); // max small items determined by size of what's below?
    //     for (int i = 0; i < nSmall; i++) {
    //         var newPos = getRandomPos(top_of_stack.y, pos.x - obj_width_halved, pos.x + obj_width_halved, pos.z - obj_length_halved, pos.z + obj_length_halved);
    //         generateSmallItem(newPos);
    //     }
    // }

    // generates one small item
    // void generateSmallItem(Vector3 pos) {
    //     GameObject obj = getRandomItem(listSmallItems);
    //     pos.y += obj.transform.localScale.y/2;
    //     instantiated.Add(Instantiate(obj, pos, Quaternion.identity));
    // }

    void generateItem(float floor, Vector4 b, GameObject[] list) {
        GameObject obj = getRandomItem(list);
        float l = obj.transform.localScale.x;
        if (l < obj.transform.localScale.z) l = obj.transform.localScale.z;
        // update bounds with obj's largest side for random position
        b += new Vector4(l/2, -l/2, l/2, -l/2);
        // b += new Vector4(obj.transform.localScale.x/2, -obj.transform.localScale.x/2, obj.transform.localScale.z/2, -obj.transform.localScale.z/2);
        floor += obj.transform.localScale.y/2;
        var pos = getRandomPos(floor, b);
        instantiated.Add(Instantiate(obj, pos, Quaternion.identity));
    }

    Vector3 getRandomPos(float floor, Vector4 b) { // xyzw = -x, x, -z, z
        //Debug.Log(b);
        return new Vector3(Random.Range(b.x, b.y), floor, Random.Range(b.z, b.w));  
    }

    GameObject getRandomItem(GameObject[] list) {
        //Debug.Log(list.Length);
        int itemIndex = Random.Range(0, list.Length-1);
        //Debug.Log(list[itemIndex].name);
        return list[itemIndex];
    }

    
}
