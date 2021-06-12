using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
