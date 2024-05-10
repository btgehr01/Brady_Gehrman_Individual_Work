using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;  // Wandering forward speed
    public float obstacleRange = 2.0f;

    private bool _alive;

    void Start()
    {
        _alive = true;
    }

    void Update()
    {
        if (!_alive) return; // this enemy may die before this enemy game object is destroyed

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
