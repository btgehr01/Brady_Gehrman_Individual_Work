using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    public int damage = 1;

    [SerializeField] private AudioClip launch;
    [SerializeField] private AudioClip hitObject;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(launch);
        StartCoroutine("SelfDestruct");
    }

    void OnCollisionEnter(Collision collisionInfo)
    {

        if (collisionInfo.gameObject.tag == "Player")
        {
            collisionInfo.gameObject.SendMessage("Hurt", 1);
            Destroy(gameObject);
        }
        else if (collisionInfo.gameObject.tag == "laser")
        {
            Destroy(gameObject);

        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(hitObject);
            Destroy(gameObject, 0.5f);
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     print("spike hit");
    //     PlayerCharacter player = other.GetComponent<PlayerCharacter>();
    //     if (player != null)
    //     {
    //         player.Hurt(damage);
    //     }
    //     Destroy(this.gameObject);
    // }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
