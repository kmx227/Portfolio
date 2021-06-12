using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Invoke("Down", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Down()
    {
        Destroy(gameObject);
    }
}
