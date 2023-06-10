using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAutoDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(AutoDestroy());
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
