using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// struct Coordinate
//     {
//         public int x;
//         public int y;
//     }
public class Test : MonoBehaviour
{
    public static GameObject[] listFurniture;
    public static GameObject[] listSmallItems;
    

    // public Coordinate c;
    // Start is called before the first frame update
    void Start()
    {
        // c.x = 5;
        // c.y = 6;
        listFurniture = Resources.LoadAll<GameObject>("Furniture");
        listSmallItems = Resources.LoadAll<GameObject>("Small Items");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            Debug.Log("size of list Furniture: " + listFurniture.Length);
            GameObject o = Instantiate(listFurniture[0], Vector3.zero, Quaternion.identity);

            var r = o.GetComponent<Renderer>();
            if (r == null) {
                Debug.Log("Null");
                return;
            }
            var bounds = r.bounds;
            Debug.Log("bounds = min " + bounds.min + "max " + bounds.max);
        }

    }
}
