using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NWH;
using NWH.Common.Input;
using NWH.Common.SceneManagement;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Input;

public class OnLoadRacingScene : MonoBehaviour
{
    public RaceManager race_mgr;

    [SerializeField] private GameObject spawnPoint;
    private VehicleChanger vehicleChanger;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the VehicleChanger component on this gameobject
        vehicleChanger = GetComponent<VehicleChanger>();

        // Get the selected car key from PlayerPrefs
        string selectedCarName = PlayerPrefs.GetString("SelectedCar");
        print("Loading vechicle: " + selectedCarName);
        //load the prefab from the Resources/CarPrefabs folder
        GameObject selectedCarPrefab = Resources.Load<GameObject>("CarPrefabs/" + selectedCarName);
        if (selectedCarPrefab == null)
        {
            Debug.LogError("Failed to load car prefab: " + selectedCarName);
        }
        else
        {
            //instantiate the carPrefab into the scene
            GameObject vehicle = Instantiate(selectedCarPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            race_mgr.player_rb.Add(vehicle.GetComponent<Rigidbody>());
            race_mgr.collider_whitelist.Add(vehicle.GetComponentInChildren<Collider>());
            VehicleController vc = vehicle.GetComponent<VehicleController>();
            SteeringWheelInputProvider sIP = vehicle.GetComponent<SteeringWheelInputProvider>();
            sIP.vehicleController = vc;
            //fetch Vehicle object from instantiated carPrefab
            Vehicle component = vehicle.GetComponent<Vehicle>();
            //add the Vehicle to the vehicles list
            vehicleChanger.vehicles.Add(component);

            // start the race
            race_mgr.start_race();
        }
    }
}
