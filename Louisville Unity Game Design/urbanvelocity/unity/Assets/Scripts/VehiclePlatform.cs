using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    private float rotationSpeed = 2.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);  // Rotate the car platform object
        transform.GetChild(0).Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);  // Rotate the child component (vehicle)
    }
}
