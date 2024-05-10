using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    void OnCollisionEnter(Collision collisionInfo)
    {

        if (collisionInfo.gameObject.tag == "enemy")
        {
            collisionInfo.gameObject.SendMessage("ReactToHit");
        }
        Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider co) // detect if it hit the enemy fire ball which has "is Trigger" on
    // {
    //     if (co.gameObject.tag == "Spike")
    //     {
    //         Destroy(co.gameObject);
    //         Destroy(gameObject);
    //     }
    // }


    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
