using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class JetController : MonoBehaviour
{
    public Transform targetJet;
    public float movementSpeed = 10f;
    public float rotationSpeed = 2f;
    public float shootingAngle = 10f;
    public float shootingDistance = 50f;
    public AudioClip explosionSound;
    public LineRenderer laserRenderer;
    public Transform laserSpawnPoint;
    public float radarUpdateTime = 5f;
    private bool gameEnded = false;
    private Vector3 lastDirectionToTarget;

    void Start()
    {
        StartCoroutine(UpdateTargetPosition());
    }

    void Update()
    {
        ApplyJetVelocityAndRotation();

        Vector3 visualDirectionToTarget = targetJet.position - transform.position;

        if (Vector3.Angle(transform.forward, visualDirectionToTarget) <= shootingAngle / 2 &&
            visualDirectionToTarget.magnitude <= shootingDistance)
        {
            Debug.Log("Laser fired");
            FireLaser();
        }
    }

    IEnumerator UpdateTargetPosition()
    {
        while (!gameEnded)
        {
            // Wait specified amount of seconds
            yield return new WaitForSeconds(radarUpdateTime);

            // Calculate and store current jet's directionToTarget
            lastDirectionToTarget = targetJet.position - transform.position;
        }
    }

    void ApplyJetVelocityAndRotation()
    {
        // Apply the forward velocity to move the jet forward
        GetComponent<Rigidbody>().velocity = transform.forward * movementSpeed;

        // Change orientation of the jet to the last known position of the other jet over time
        if (lastDirectionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastDirectionToTarget);

            // Calculate the rotation difference
            float rotationDifference = Quaternion.Angle(transform.rotation, targetRotation);

            // Check if the rotation difference is below a threshold to consider it as reached
            if (rotationDifference < 0.5f)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void FireLaser()
    {
        // Create a laser from the spawn point in the direction of the target
        RaycastHit hit;
        if (Physics.Raycast(laserSpawnPoint.position, transform.forward, out hit, shootingDistance))
        {
            // Check if the laser hits the target
            if (hit.transform.CompareTag("Jet"))
            {
                Debug.Log("Laser hit the target!");
                Destroy(hit.transform.gameObject);
            }
        }

        // Display the laser visually using LineRenderer
        laserRenderer.SetPosition(0, laserSpawnPoint.position);
        laserRenderer.SetPosition(1, laserSpawnPoint.position + transform.forward * shootingDistance);

        // Reset the LineRenderer after a short delay to simulate an instant laser
        Invoke("ResetLaser", 0.1f);
    }

    void ResetLaser()
    {
        laserRenderer.SetPosition(0, Vector3.zero);
        laserRenderer.SetPosition(1, Vector3.zero);
    }

    void EndGame()
    {
        // Exit Play Mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
