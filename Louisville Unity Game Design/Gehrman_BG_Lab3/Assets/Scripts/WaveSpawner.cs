using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefab2;
    public float startTime;
    public float endTime;
    public float spawnRate;
    private System.Random rand = new System.Random();

    void Start()
    {
        InvokeRepeating("Spawn", startTime, spawnRate);
        Invoke("EndSpawner", endTime);
    }

    void Spawn()
    {
        if (rand.Next(2) == 0)
            Instantiate(prefab, transform.position, transform.rotation);
        else
            Instantiate(prefab2, transform.position, transform.rotation);
    }

    void EndSpawner()
    {
        CancelInvoke();
    }
}

