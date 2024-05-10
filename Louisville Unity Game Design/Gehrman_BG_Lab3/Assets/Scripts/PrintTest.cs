using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTest : MonoBehaviour
{
    public string id;
    int n = 0;

    // Start is called before the first frame update
    void Start()
    {
        print(id + " Start call");
    }

    // Update is called once per frame
    void Update()
    {
        print(n.ToString() + "-th Update calls : " + id);
        n++;
    }
}
