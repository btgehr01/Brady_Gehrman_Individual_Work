using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public GameObject Laser;

    private float Velocity = 100f;
    public GameObject ShootingPoint;

    private int ammo;
    private bool cursorLocked = false;

    [SerializeField] private AudioClip shoot;

    [SerializeField] private AudioClip emptyMag;

    [SerializeField] private AudioClip reload;

    void Start()
    {
        LockCursor();
        ammo = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                GameObject laser = (GameObject)Instantiate(Laser, ShootingPoint.transform.position,
                    transform.rotation);
                laser.name = "laser";
                Rigidbody laser_rb = laser.GetComponent<Rigidbody>();
                GetComponent<AudioSource>().PlayOneShot(shoot);
                laser_rb.velocity = transform.TransformDirection(new Vector3(0, 0, Velocity));
                ammo = ammo - 1;
                // print("Mag: " + ammo);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(emptyMag);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            LockCursor();
        }
        if (Input.GetKeyDown(KeyCode.R) && ammo < 10)
        {
            GetComponent<AudioSource>().PlayOneShot(reload);
            ammo = 10;
            // print("Mag: " + ammo);
        }

    }

    void LockCursor()
    {
        if (!cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLocked = false;
        }
    }
}
