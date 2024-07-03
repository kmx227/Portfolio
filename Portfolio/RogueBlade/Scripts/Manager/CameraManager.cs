using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float moveSpeed; 
    private Vector3 targetPosition; 

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            targetPosition.y = math.clamp(targetPosition.y, 3, 100);
            transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
