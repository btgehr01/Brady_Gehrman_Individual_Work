using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannan_Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the cannon ball has hit the pirate ship
        if (collision.gameObject.tag == "ship")
        {

            print("Target hit!");

            // Destroy the cannon ball
            Destroy(transform);
        }
    }
}
