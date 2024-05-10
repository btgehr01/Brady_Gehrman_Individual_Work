using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    public Transform cannonBallSpawnPoint;
    public float rotationSpeed;
    public float firingVelocity;

    void Update()
    {
        //left and right arrow key input
        float horizontalInput = Input.GetAxis("Horizontal");
        //up and down arrow key input
        float verticalInput = Input.GetAxis("Vertical");

        // cannon horizontal rotation limit
        if ((transform.localEulerAngles.y + horizontalInput * rotationSpeed * Time.deltaTime > 90f && transform.localEulerAngles.y + horizontalInput * rotationSpeed * Time.deltaTime < 270f))
        {
            print("Horizontal Rotation Limit Reached");
        }
        else
        {
            transform.Rotate(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f, Space.World);
        }

        // cannon vertical rotation limit
        if ((transform.localEulerAngles.x + (-1 * verticalInput * rotationSpeed * Time.deltaTime) > 0f && transform.localEulerAngles.x + (-1 * verticalInput * rotationSpeed * Time.deltaTime) < 270f))
        {
            print("Vertical Rotation Limit Reached");
        }
        else
        {
            transform.Rotate(-1 * verticalInput * rotationSpeed * Time.deltaTime, 0f, 0f, Space.Self);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Instantiate the cannon ball at the spawn point
            GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonBallSpawnPoint.position, transform.rotation);

            // Set the velocity of the cannon ball based on the cannon's forward orentation and the selected velocity
            cannonBall.GetComponent<Rigidbody>().velocity = transform.forward * firingVelocity;
        }
    }
}
