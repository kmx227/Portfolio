using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOut : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(CloseUI());
    }

    IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
