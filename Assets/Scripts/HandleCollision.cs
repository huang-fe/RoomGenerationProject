using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCollision : MonoBehaviour
{
    // not actually handling collision, this is adding random rotations
    void Start() {
        int r = Random.Range(0, 3);
        for (int i = 0; i < r; i++) {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // GameObject o = collision.gameObject;
        // Debug.Log("collision : " + o.name);
        // if (collision.gameObject.name == "Wall 1") {
        //     transform.Translate(Vector3.left); // move toward -x
        // }
        // if (collision.gameObject.name == "Wall 2") {
        //     transform.Translate(Vector3.right); // move toward x
        // }
        // if (collision.gameObject.name == "Wall 3") {
        //     transform.Translate(Vector3.back); // move toward -z
        // }
        // if (collision.gameObject.name == "Wall 4") {
        //     transform.Translate(Vector3.forward); // move toward z
        // }
        // if (collision.gameObject.name == "Floor") {
        //     transform.Translate(Vector3.up); 
        // }
        

        // //Check for a match with the specific tag on any GameObject that collides with your GameObject
        // if (collision.gameObject.tag == "MyGameObjectTag")
        // {
        //     //If the GameObject has the same tag as specified, output this message in the console
        //     Debug.Log("Do something else here");
        // }
    }
}
