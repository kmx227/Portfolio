using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        var dist = Vector2.Distance(transform.transform.position, target.transform.position);

        if (dist < 0.07f) transform.localScale = new Vector3(0.2f, 1.1f, 0f);
        else if (dist > 1f) transform.localScale = Vector3.zero;
        else {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, dist * Time.deltaTime);
        }
    }
}
