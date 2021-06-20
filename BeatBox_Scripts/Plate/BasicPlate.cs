using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlate : MonoBehaviour
{
    //깃발을 꽂아서 방향설정
    public bool flag = true;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("Move", 2f);
        }
    }

    void Move()
    {
        Destroy(gameObject);
    }
}