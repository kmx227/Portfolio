using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveST : MonoBehaviour
{
    public Transform port;
    PlayerController thePlayer;
    int MaxDistance = 15;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Ray()
    {

        if (Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 0.3f);
                transform.position = t_hitInfo.transform.position;
            }


            //transform.position = port.position;
        }
    }
}
